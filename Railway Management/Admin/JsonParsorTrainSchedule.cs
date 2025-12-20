using Newtonsoft.Json;
using static Railway_Management.Models.AllDataDetails;
using System.Reflection;
using System.Data;
using System.Globalization;
using System.Text;
using ExcelDataReader;
using Npgsql;

namespace Railway_Management.Admin
{
    public class JsonParsorTrainSchedule
    {
        /// <summary>
        /// Read an Excel (.xlsx/.xls) or CSV file and map rows to a List&lt;TrainSchedule&gt; using header-to-property matching.
        /// Returns an empty list if no rows found or on failure (exceptions are written to Console).
        /// Requires ExcelDataReader + ExcelDataReader.DataSet NuGet packages to read .xls/.xlsx.
        /// </summary>
        public List<TrainSchedule> jsonParseScheduleTain(string path)
        {
            var result = new List<TrainSchedule>();

            try
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    Console.WriteLine("File not found: " + path);
                    return result;
                }

                var ext = Path.GetExtension(path).ToLowerInvariant();

                DataTable table = null;

                if (ext == ".csv")
                {
                    table = ReadCsvToDataTable(path);
                }
                else if (ext == ".xls" || ext == ".xlsx")
                {
                    // ExcelDataReader required:
                    // Install-Package ExcelDataReader
                    // Install-Package ExcelDataReader.DataSet
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using var reader = ExcelReaderFactory.CreateReader(stream);
                    var dsConfig = new ExcelDataReader.ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataReader.ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };
                    var ds = reader.AsDataSet(dsConfig);
                    if (ds.Tables.Count > 0) table = ds.Tables[0];
                }
                else
                {
                    Console.WriteLine("Unsupported file extension: " + ext);
                    return result;
                }

                if (table == null || table.Rows.Count == 0)
                    return result;

                // Map headers to TrainSchedule properties (case-insensitive)
                var scheduleType = typeof(TrainSchedule);
                var props = scheduleType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var propLookup = props.ToDictionary(p => p.Name.ToLowerInvariant(), p => p);

                var headerMap = new Dictionary<int, PropertyInfo>();
                for (int c = 0; c < table.Columns.Count; c++)
                {
                    var header = table.Columns[c].ColumnName?.Trim();
                    if (string.IsNullOrEmpty(header)) continue;
                    // try to match header to property name (exact or case-insensitive)
                    var key = header.Replace(" ", "").Replace("_", "").ToLowerInvariant();
                    var matched = props.FirstOrDefault(p =>
                        p.Name.Replace(" ", "").Replace("_", "").ToLowerInvariant() == key
                        || p.Name.ToLowerInvariant() == header.ToLowerInvariant()
                        || p.Name.ToLowerInvariant() == header.Replace(" ", "").ToLowerInvariant()
                    );
                    if (matched != null) headerMap[c] = matched;
                    else
                    {
                        // Try direct property name match
                        if (propLookup.TryGetValue(header.ToLowerInvariant(), out var pi)) headerMap[c] = pi;
                    }
                }

                foreach (DataRow row in table.Rows)
                {
                    var instance = Activator.CreateInstance<TrainSchedule>();
                    foreach (var kv in headerMap)
                    {
                        var colIndex = kv.Key;
                        var prop = kv.Value;
                        try
                        {
                            var raw = row[colIndex];
                            if (raw == null || raw == DBNull.Value) continue;

                            object valueToSet = null;
                            var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                            if (targetType == typeof(string))
                            {
                                valueToSet = raw.ToString();
                            }
                            else if (targetType == typeof(int))
                            {
                                if (int.TryParse(raw.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var i)) valueToSet = i;
                            }
                            else if (targetType == typeof(long))
                            {
                                if (long.TryParse(raw.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var l)) valueToSet = l;
                            }
                            else if (targetType == typeof(decimal))
                            {
                                if (decimal.TryParse(raw.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var d)) valueToSet = d;
                            }
                            else if (targetType == typeof(double))
                            {
                                if (double.TryParse(raw.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var dd)) valueToSet = dd;
                            }
                            else if (targetType == typeof(bool))
                            {
                                if (bool.TryParse(raw.ToString(), out var b)) valueToSet = b;
                                else
                                {
                                    var s = raw.ToString().Trim();
                                    if (s == "1" || s.Equals("yes", StringComparison.OrdinalIgnoreCase) || s.Equals("y", StringComparison.OrdinalIgnoreCase)) valueToSet = true;
                                    if (s == "0" || s.Equals("no", StringComparison.OrdinalIgnoreCase) || s.Equals("n", StringComparison.OrdinalIgnoreCase)) valueToSet = false;
                                }
                            }
                            else if (targetType == typeof(DateTime))
                            {
                                if (DateTime.TryParse(raw.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt)) valueToSet = dt;
                            }
                            else
                            {
                                // fallback: try change type
                                try
                                {
                                    valueToSet = Convert.ChangeType(raw, targetType, CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    // ignore if conversion fails
                                }
                            }

                            if (valueToSet != null)
                                prop.SetValue(instance, valueToSet);
                        }
                        catch (Exception ex)
                        {
                            // continue mapping other columns; log for diagnosis
                            Console.WriteLine($"Mapping error for column {colIndex} -> property {kv.Value.Name}: {ex.Message}");
                        }
                    }

                    result.Add(instance);
                }

                return result;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Console.WriteLine(Ex.InnerException?.Message);
                return new List<TrainSchedule>();
            }
        }

        private DataTable ReadCsvToDataTable(string path)
        {
            var dt = new DataTable();
            using var sr = new StreamReader(path, Encoding.UTF8);
            string? headerLine = sr.ReadLine();
            if (headerLine == null) return dt;

            var headers = SplitCsvLine(headerLine);
            foreach (var h in headers) dt.Columns.Add(h.Trim());

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;
                var cols = SplitCsvLine(line);
                var row = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count && i < cols.Length; i++)
                {
                    row[i] = cols[i];
                }
                dt.Rows.Add(row);
            }

            return dt;
        }

        // Simple CSV splitter handling quoted values
        private string[] SplitCsvLine(string line)
        {
            var values = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                var ch = line[i];
                if (ch == '"' )
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    values.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(ch);
                }
            }
            values.Add(sb.ToString());
            return values.ToArray();
        }

        /// <summary>
        /// High-performance bulk insert: streams rows from Excel/CSV directly into PostgreSQL using COPY (binary).
        /// - connectionString: Npgsql connection string to your PostgreSQL database.
        /// - tableName: target table name in PostgreSQL (use exact table name, schema-qualified if needed).
        /// NOTE: Excel/CSV header names should match PostgreSQL column names (case-sensitive if quoted).
        /// Requires Npgsql NuGet package.
        /// </summary>
        public async Task<long> BulkInsertToPostgresAsync(string path, string connectionString, string tableName = "TrainSchedule")
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                Console.WriteLine("File not found: " + path);
                return 0;
            }

            var ext = Path.GetExtension(path).ToLowerInvariant();
            List<string> headers;
            IEnumerable<IList<object?>> rowSource;

            // Prepare streaming row source and headers
            if (ext == ".csv")
            {
                using var sr = new StreamReader(path, Encoding.UTF8);
                var headerLine = await sr.ReadLineAsync().ConfigureAwait(false);
                if (headerLine == null) return 0;
                headers = SplitCsvLine(headerLine).Select(h => h.Trim()).Where(h => !string.IsNullOrEmpty(h)).ToList();

                // Build row enumerator that yields IList<object?> per row
                rowSource = GetCsvRowEnumerator(path, headers.Count);
            }
            else if (ext == ".xls" || ext == ".xlsx")
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var reader = ExcelReaderFactory.CreateReader(stream);
                // Read first row as header
                if (!reader.Read())
                {
                    reader.Dispose();
                    stream.Dispose();
                    return 0;
                }

                headers = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var val = reader.GetValue(i);
                    headers.Add(val?.ToString()?.Trim() ?? $"Column{i + 1}");
                }

                // Create an enumerable that yields rows from the Excel reader then disposes resources when finished
                rowSource = GetExcelRowEnumerator(reader, stream);
            }
            else
            {
                Console.WriteLine("Unsupported file extension: " + ext);
                return 0;
            }

            // Escape/quote column names for COPY command (double-quote identifiers)
            var quotedCols = headers.Select(h => "\"" + h.Replace("\"", "\"\"") + "\"");
            var copySql = $"COPY \"{tableName.Replace("\"", "\"\"")}\" ({string.Join(", ", quotedCols)}) FROM STDIN (FORMAT BINARY)";

            long count = 0;
            // Execute COPY using Npgsql binary importer
            await using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync().ConfigureAwait(false);
            using (var importer = conn.BeginBinaryImport(copySql))
            {
                foreach (var row in rowSource)
                {
                    importer.StartRow();
                    for (int i = 0; i < headers.Count; i++)
                    {
                        var value = i < row.Count ? row[i] : null;
                        if (value == null || value == DBNull.Value) importer.WriteNull();
                        else
                        {
                            // Let Npgsql infer type from CLR object. Convert DateTime to UTC kind if needed.
                            if (value is string s) importer.Write(s);
                            else if (value is double d) importer.Write(d);
                            else if (value is float f) importer.Write(f);
                            else if (value is decimal dec) importer.Write(dec);
                            else if (value is int ii) importer.Write(ii);
                            else if (value is long ll) importer.Write(ll);
                            else if (value is bool bb) importer.Write(bb);
                            else if (value is DateTime dt) importer.Write(dt);
                            else importer.Write(value);
                        }
                    }
                    count++;
                }
                importer.Complete();
            }

            return count;
        }

        // Enumerator for CSV rows (yields one IList<object?> per row)
        private IEnumerable<IList<object?>> GetCsvRowEnumerator(string path, int expectedCols)
        {
            using var sr = new StreamReader(path, Encoding.UTF8);
            // skip header
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (string.IsNullOrEmpty(line)) continue;
                var cols = SplitCsvLine(line);
                var row = new List<object?>(expectedCols);
                for (int i = 0; i < expectedCols; i++)
                {
                    row.Add(i < cols.Length ? (object?)cols[i] : null);
                }
                yield return row;
            }
        }

        // Enumerator for Excel rows reading directly from ExcelDataReader (streaming)
        private IEnumerable<IList<object?>> GetExcelRowEnumerator(IExcelDataReader reader, FileStream stream)
        {
            try
            {
                // First row already read as header by caller, so start reading subsequent rows
                while (reader.Read())
                {
                    var list = new List<object?>(reader.FieldCount);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var v = reader.GetValue(i);
                        list.Add(v);
                    }
                    yield return list;
                }
            }
            finally
            {
                // Ensure resources disposed
                reader.Close();
                reader.Dispose();
                stream.Dispose();
            }
        }
    }
}

