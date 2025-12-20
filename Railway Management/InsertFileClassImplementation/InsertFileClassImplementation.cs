using CsvHelper;
using Npgsql;
using NuGet.Protocol;
using System.Globalization;
using System.IO.Pipelines;
using System.Text;

namespace Railway_Management.InsertFileClassImplementation
{
    public static class InsertFileClassImplementation
    {
        public static async Task UploadCsvAsync(string csvPath, string _connectionString)
        {
            var logFilePath = Path.ChangeExtension(csvPath, ".import.log");

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"
            COPY raw_train_stops(
                trainno,
                trainname,
                seq,
                stationcode,
                stationname,
                arrivaltime,
                departuretime,
                distance,
                sourcestation,
                sourcestationname,
                destinationstation,
                destinationstationname

            )
            FROM STDIN (FORMAT CSV, HEADER TRUE)
        ";

            await using var writer = conn.BeginTextImport(sql);
            using var reader = new StreamReader(csvPath);
            using var logWriter = new StreamWriter(logFilePath, append: true, Encoding.UTF8);

            int lineNumber = 0;

            try
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    lineNumber++;

                    // Write to DB
                    await writer.WriteLineAsync(line);

                    // Write to log file
                    await logWriter.WriteLineAsync($"LINE {lineNumber}: {line}");
                }
            }
            catch (Exception ex)
            {
                await logWriter.WriteLineAsync("----- ERROR OCCURRED -----");
                await logWriter.WriteLineAsync($"Line Number: {lineNumber}");
                await logWriter.WriteLineAsync($"CSV Line: {reader.ReadLine()}");
                await logWriter.WriteLineAsync($"Error: {ex.Message}");
                await logWriter.WriteLineAsync($"Time: {DateTime.UtcNow}");

                throw; // rethrow so upper layer knows import failed
            }


        }
    }
}