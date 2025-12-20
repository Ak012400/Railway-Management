Railway Management/Utilities/ImagePdfConverter.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats;

namespace Railway_Management.Utilities
{
    // Requires NuGet: PdfSharpCore, SixLabors.ImageSharp
    public static class ImagePdfConverter
    {
        /// <summary>
        /// Convert multiple image files to a single PDF. Streams images, disposes resources quickly.
        /// Good for large image sets.
        /// </summary>
        public static async Task ImagesToPdfAsync(IEnumerable<string> imagePaths, string outputPdfPath, CancellationToken cancellationToken = default)
        {
            if (imagePaths == null) throw new ArgumentNullException(nameof(imagePaths));
            Directory.CreateDirectory(Path.GetDirectoryName(outputPdfPath) ?? string.Empty);

            using var document = new PdfDocument();
            foreach (var imagePath in imagePaths)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!File.Exists(imagePath)) continue;

                // Load image with ImageSharp (streamed)
                using var imgStream = File.OpenRead(imagePath);
                using var image = await Image.LoadAsync<Rgba32>(imgStream, cancellationToken).ConfigureAwait(false);

                // Create page matching image ratio (A4 fallback)
                var dpi = 72.0; // PDF points per inch
                var widthPt = image.Width * 72.0 / image.Metadata.HorizontalResolution;
                var heightPt = image.Height * 72.0 / image.Metadata.VerticalResolution;
                if (double.IsNaN(widthPt) || widthPt <= 0) widthPt = image.Width * 0.75;
                if (double.IsNaN(heightPt) || heightPt <= 0) heightPt = image.Height * 0.75;

                var page = document.AddPage();
                page.Width = widthPt;
                page.Height = heightPt;

                using var gfx = XGraphics.FromPdfPage(page);
                // Convert ImageSharp image to System.Drawing-like stream for PdfSharpCore
                using var ms = new MemoryStream();
                await image.SaveAsPngAsync(ms, cancellationToken).ConfigureAwait(false);
                ms.Position = 0;
                using var ximg = XImage.FromStream(() => ms);
                gfx.DrawImage(ximg, 0, 0, page.Width, page.Height);
            }

            // Save pdf
            using var outStream = File.Open(outputPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            document.Save(outStream);
        }
    }
}