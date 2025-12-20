using ExcelDataReader;
using ClosedXML.Excel;
using System.Text;

namespace Railway_Management.Utilities
{
    // Requires NuGet: ExcelDataReader, ExcelDataReader.DataSet
    public static class ExcelCsvConverter
    {
        /// <summary>
        /// Streams an Excel (.xls/.xlsx) file to CSV. Fast and low-memory: reads rows and writes them immediately.
        /// </summary>
        public static async Task ExcelToCsvAsync(string excelPath, string csvPath, char delimiter = ',', CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(excelPath)) throw new ArgumentNullException(nameof(excelPath));
            if (string.IsNullOrWhiteSpace(csvPath)) throw new ArgumentNullException(nameof(csvPath));
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            // Create output directory if needed
            Directory.CreateDirectory(Path.GetDirectoryName(csvPath) ?? string.Empty);

            // Use buffered StreamWriter for speed
            using var sw = new StreamWriter(new FileStream(csvPath, FileMode.Create, FileAccess.Write, FileShare.None, 64 * 1024), Encoding.UTF8);
            // Read each sheet sequentially - write sheet separator row if multiple sheets exist
            do
            {
                // Skip empty sheets
                if (!reader.Read()) continue;

                // Optionally write sheet header row (commented by default)
                // await sw.WriteLineAsync($"#Sheet:{reader.Name}").ConfigureAwait(false);

                // Build header from first row if UseHeaderRow false; ExcelDataReader doesn't provide direct header flag here so simply write each row
                do
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var values = new object?[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values[i] = reader.GetValue(i);
                    }

                    // Escape values and write CSV line
                    await sw.WriteLineAsync(BuildCsvLine(values, delimiter)).ConfigureAwait(false);
                }
                while (reader.Read());
            }
            while (reader.NextResult());

            await sw.FlushAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Converts a CSV file to an Excel (.xlsx) file. Streams lines and writes to ClosedXML workbook.
        /// </summary>
        public static async Task CsvToExcelAsync(string csvPath, string excelPath, char delimiter = ',', CancellationToken cancellationToken = default)
        {
            if (!File.Exists(csvPath)) throw new FileNotFoundException("CSV not found", csvPath);
            Directory.CreateDirectory(Path.GetDirectoryName(excelPath) ?? string.Empty);

            // Lazy-load ClosedXML only when this method is called (reduce surface if not used).
            // Requires NuGet: ClosedXML
            using var reader = new StreamReader(new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.UTF8);
            var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Sheet1");
            int row = 1;

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var line = await reader.ReadLineAsync().ConfigureAwait(false);
                if (line == null) break;
                var cols = SplitCsvLine(line, delimiter);
                for (int c = 0; c < cols.Length; c++)
                {
                    ws.Cell(row, c + 1).Value = cols[c];
                }
                row++;
            }

            workbook.SaveAs(excelPath);
        }

        private static string BuildCsvLine(object?[] values, char delimiter)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                var v = values[i]?.ToString() ?? string.Empty;
                bool mustQuote = v.Contains(delimiter) || v.Contains('"') || v.Contains('\n') || v.Contains('\r');
                if (mustQuote)
                {
                    sb.Append('"');
                    sb.Append(v.Replace("\"", "\"\""));
                    sb.Append('"');
                }
                else
                {
                    sb.Append(v);
                }
                if (i < values.Length - 1) sb.Append(delimiter);
            }
            return sb.ToString();
        }

        private static string[] SplitCsvLine(string line, char delimiter)
        {
            if (string.IsNullOrEmpty(line)) return Array.Empty<string>();
            var values = new System.Collections.Generic.List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                var ch = line[i];
                if (ch == '"')
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
                else if (ch == delimiter && !inQuotes)
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
    }
}
