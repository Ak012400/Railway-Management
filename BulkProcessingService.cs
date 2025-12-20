Railway Management/Services/BulkProcessingService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Railway_Management.Services
{
    using Utilities;

    public class BulkProcessingService
    {
        private readonly string _connectionString;

        public BulkProcessingService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Convert Excel -> CSV and execute provided CSV-processor (for example: COPY bulk insert implementation).
        /// The csvProcessor should accept the absolute CSV path and perform a fast bulk operation.
        /// </summary>
        public async Task<long> ConvertExcelAndBulkInsertAsync(string excelPath, Func<string, Task<long>> csvProcessor, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(excelPath)) throw new FileNotFoundException("Excel file not found", excelPath);
            var csvPath = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            try
            {
                await ExcelCsvConverter.ExcelToCsvAsync(excelPath, csvPath, ',', cancellationToken).ConfigureAwait(false);
                // csvProcessor should perform the fastest available bulk load (COPY, Npgsql binary importer, etc.)
                var count = await csvProcessor(csvPath).ConfigureAwait(false);
                return count;
            }
            finally
            {
                try { File.Delete(csvPath); } catch { /* ignore cleanup errors */ }
            }
        }

        /// <summary>
        /// Convert images to a single PDF. Returns output PDF file path.
        /// </summary>
        public async Task<string> ConvertImagesToPdfAsync(IEnumerable<string> imagePaths, string outputPdfPath, CancellationToken cancellationToken = default)
        {
            await ImagePdfConverter.ImagesToPdfAsync(imagePaths, outputPdfPath, cancellationToken).ConfigureAwait(false);
            return outputPdfPath;
        }

        /// <summary>
        /// Merge images into single image, then optionally convert merged image to PDF.
        /// </summary>
        public async Task<string> MergeImagesAndOptionallyToPdfAsync(IEnumerable<string> imagePaths, string mergedOutputPath, bool exportPdf = false, string? pdfOutputPath = null, CancellationToken cancellationToken = default)
        {
            var merged = await ImageMerger.MergeImagesAsync(imagePaths, mergedOutputPath, columns: 2, spacing: 8, cancellationToken).ConfigureAwait(false);
            if (exportPdf)
            {
                if (string.IsNullOrWhiteSpace(pdfOutputPath)) pdfOutputPath = Path.ChangeExtension(mergedOutputPath, ".pdf");
                await ImagePdfConverter.ImagesToPdfAsync(new[] { merged }, pdfOutputPath, cancellationToken).ConfigureAwait(false);
                return pdfOutputPath;
            }
            return merged;
        }
    }
}