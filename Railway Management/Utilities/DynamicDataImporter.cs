using CsvHelper;
using Npgsql;
using System.Globalization;
using System.Text.Json;
using System.Text;
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

#if USE_EXCEL
using ExcelDataReader;
#endif

namespace Railway_Management.Utilities
{
    public static class DynamicDataImporter
    {
        /// <summary>
        /// Import data from CSV/JSON/Excel into PostgreSQL dynamically:
        /// - Infers columns by union of keys (JSON) or header (CSV/Excel).
        /// - Infers simple SQL types (integer, numeric, boolean, timestamp, time, text).
        /// - Creates table if not exists and inserts rows using PostgreSQL binary COPY for throughput.
        /// </summary>
        public static async Task ImportFileAsync(string filePath, string tableName, string connectionString)
        {
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            List<Dictionary<string, string?>> rows;

            if (ext == ".csv")
                rows = ReadCsv(filePath);
            else if (ext == ".json")
                rows = ReadJson(filePath);
            else if (ext == ".xls" || ext == ".xlsx")
                rows = ReadExcel(filePath);
            else
                throw new NotSupportedException($"File type '{ext}' is not supported.");

            if (rows.Count == 0)
                return;

            var orderedColumns = GetOrderedColumns(rows, filePath);

            var columnTypes = orderedColumns.ToDictionary(
                c => c,
                c => InferSqlType(rows.Select(r => r.ContainsKey(c) ? r[c] : null))
            );

            await EnsureTableAsync(tableName, columnTypes, connectionString).ConfigureAwait(false);
            await InsertRowsAsync(tableName, orderedColumns, rows, columnTypes, connectionString).ConfigureAwait(false);
        }

        private static List<Dictionary<string, string?>> ReadCsv(string path)
        {
            using var reader = new StreamReader(path, Encoding.UTF8);
            var cfg = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null,
                IgnoreBlankLines = true
            };

            using var csv = new CsvReader(reader, cfg);
            var result = new List<Dictionary<string, string?>>();

            if (!csv.Read() || !csv.ReadHeader())
                return result;

            var headers = csv.HeaderRecord;

            while (csv.Read())
            {
                var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                foreach (var h in headers)
                {
                    string value;
                    try { value = csv.GetField(h); }
                    catch { value = null; }
                    dict[h ?? string.Empty] = string.IsNullOrWhiteSpace(value) ? null : value;
                }
                result.Add(dict);
            }

            return result;
        }

        private static List<Dictionary<string, string?>> ReadJson(string path)
        {
            var json = File.ReadAllText(path, Encoding.UTF8);
            using var doc = JsonDocument.Parse(json);
            var result = new List<Dictionary<string, string?>>();

            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in doc.RootElement.EnumerateArray())
                {
                    if (el.ValueKind == JsonValueKind.Object)
                    {
                        var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                        FlattenJson(el, "", dict);
                        result.Add(dict);
                    }
                }
            }
            else if (doc.RootElement.ValueKind == JsonValueKind.Object)
            {
                var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                FlattenJson(doc.RootElement, "", dict);
                result.Add(dict);
            }

            return result;
        }

        private static List<Dictionary<string, string?>> ReadExcel(string path)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = new List<Dictionary<string, string?>>();

            var header = new List<string>();
            var first = true;
            while (reader.Read())
            {
                if (first)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        header.Add(reader.GetValue(i)?.ToString() ?? $"Column{i}");
                    }
                    first = false;
                    continue;
                }

                var dict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < header.Count; i++)
                {
                    var raw = reader.GetValue(i)?.ToString();
                    dict[header[i]] = string.IsNullOrWhiteSpace(raw) ? null : raw;
                }
                result.Add(dict);
            }

            return result;
        }

        private static void FlattenJson(JsonElement el, string prefix, Dictionary<string, string?> outDict)
        {
            if (el.ValueKind != JsonValueKind.Object)
                return;

            foreach (var prop in el.EnumerateObject())
            {
                var key = string.IsNullOrEmpty(prefix) ? prop.Name : prefix + "." + prop.Name;
                var v = prop.Value;

                if (v.ValueKind == JsonValueKind.Object)
                {
                    FlattenJson(v, key, outDict);
                }
                else if (v.ValueKind == JsonValueKind.Array)
                {
                    var items = new List<string>();
                    foreach (var item in v.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String || item.ValueKind == JsonValueKind.Number || item.ValueKind == JsonValueKind.True || item.ValueKind == JsonValueKind.False)
                            items.Add(item.ToString());
                    }
                    outDict[key] = items.Count == 0 ? null : string.Join(",", items);
                }
                else
                {
                    outDict[key] = v.ValueKind == JsonValueKind.Null ? null : v.ToString();
                }
            }
        }

        private static List<string> GetOrderedColumns(List<Dictionary<string, string?>> rows, string filePath)
        {
            var first = rows.FirstOrDefault();
            if (first == null)
                return new List<string>();

            var ordered = new List<string>(first.Keys);

            foreach (var r in rows)
            {
                foreach (var k in r.Keys)
                {
                    if (!ordered.Contains(k))
                        ordered.Add(k);
                }
            }

            return ordered;
        }

        private static string InferSqlType(IEnumerable<string?> values)
        {
            bool allInt = true, allNumeric = true, allBool = true, allDateTime = true, allTimeSpan = true;
            foreach (var v in values)
            {
                if (string.IsNullOrWhiteSpace(v))
                    continue;

                if (allInt && !int.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                    allInt = false;

                if (allNumeric && !decimal.TryParse(v, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                    allNumeric = false;

                if (allBool && !bool.TryParse(v, out _))
                    allBool = false;

                if (allDateTime && !DateTime.TryParse(v, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    allDateTime = false;

                if (allTimeSpan && !TimeSpan.TryParse(v, CultureInfo.InvariantCulture, out _))
                    allTimeSpan = false;

                if (!allInt && !allNumeric && !allBool && !allDateTime && !allTimeSpan)
                    break;
            }

            if (allInt) return "integer";
            if (allNumeric) return "numeric";
            if (allBool) return "boolean";
            if (allTimeSpan) return "time";
            if (allDateTime) return "timestamp";
            return "text";
        }

        private static async Task EnsureTableAsync(string tableName, Dictionary<string, string> columnTypes, string connString)
        {
            var sb = new StringBuilder();
            sb.Append($"CREATE TABLE IF NOT EXISTS {QuoteIdentifier(tableName)} (");
            var cols = columnTypes.Select(kv => $"{QuoteIdentifier(kv.Key)} {kv.Value}");
            sb.Append(string.Join(", ", cols));
            sb.Append(");");

            await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync().ConfigureAwait(false);
            await using var cmd = new NpgsqlCommand(sb.ToString(), conn);
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        private const int DefaultBatchSize = 50000;

        private static async Task InsertRowsAsync(string tableName, List<string> columns,
            List<Dictionary<string, string?>> rows, Dictionary<string, string> columnTypes, string connString)
        {
            if (rows.Count == 0) return;

            var csb = new NpgsqlConnectionStringBuilder(connString)
            {
                Pooling = false
            };
            var localConnString = csb.ToString();

            var quotedCols = string.Join(", ", columns.Select(QuoteIdentifier));

            for (var offset = 0; offset < rows.Count; offset += DefaultBatchSize)
            {
                var batchSize = Math.Min(DefaultBatchSize, rows.Count - offset);

                // avoid copying large lists where possible; iterate indices
                await using var conn = new NpgsqlConnection(localConnString);
                await conn.OpenAsync().ConfigureAwait(false);

                await using var tran = await conn.BeginTransactionAsync().ConfigureAwait(false);

                NpgsqlBinaryImporter? importer = null;
                try
                {
                    importer = conn.BeginBinaryImport($"COPY {QuoteIdentifier(tableName)} ({quotedCols}) FROM STDIN (FORMAT BINARY)");

                    for (var i = 0; i < batchSize; i++)
                    {
                        var row = rows[offset + i];
                        importer.StartRow();

                        for (var c = 0; c < columns.Count; c++)
                        {
                            var col = columns[c];
                            var raw = row.TryGetValue(col, out var v) ? v : null;
                            var type = columnTypes[col];

                            if (string.IsNullOrWhiteSpace(raw))
                            {
                                importer.WriteNull();
                                continue;
                            }

                            switch (type)
                            {
                                case "integer":
                                    if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var iv))
                                        importer.Write(iv, NpgsqlTypes.NpgsqlDbType.Integer);
                                    else
                                        importer.WriteNull();
                                    break;
                                case "numeric":
                                    if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var dv))
                                        importer.Write(dv, NpgsqlTypes.NpgsqlDbType.Numeric);
                                    else
                                        importer.WriteNull();
                                    break;
                                case "boolean":
                                    if (bool.TryParse(raw, out var bv))
                                        importer.Write(bv, NpgsqlTypes.NpgsqlDbType.Boolean);
                                    else
                                        importer.WriteNull();
                                    break;
                                case "timestamp":
                                    if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                                        importer.Write(dt, NpgsqlTypes.NpgsqlDbType.Timestamp);
                                    else
                                        importer.WriteNull();
                                    break;
                                case "time":
                                    if (TimeSpan.TryParse(raw, CultureInfo.InvariantCulture, out var ts))
                                        importer.Write(ts, NpgsqlTypes.NpgsqlDbType.Time);
                                    else
                                        importer.WriteNull();
                                    break;
                                default:
                                    importer.Write(raw, NpgsqlTypes.NpgsqlDbType.Text);
                                    break;
                            }
                        }
                    }

                    // Ensure the binary import is fully finished before committing the transaction.
                    // Use the async API so the connector is not still busy when CommitAsync starts.
                    await importer.CompleteAsync().ConfigureAwait(false);
                    await tran.CommitAsync().ConfigureAwait(false);
                }
                catch(Exception ex)
                {
                    try
                    {
                        if (importer != null)
                            await importer.CloseAsync().ConfigureAwait(false);
                    }
                    catch
                    {
                        // best-effort
                    }

                    try
                    {
                        await tran.RollbackAsync().ConfigureAwait(false);
                    }
                    catch
                    {
                        // best-effort
                    }

                    throw;
                }
                finally
                {
                    if (importer != null)
                        await importer.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        private static string QuoteIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                identifier = "col";
            var safe = identifier.Replace("\"", "\"\"");
            return $"\"{safe}\"";
        }
    }
}