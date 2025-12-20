Railway Management/Utilities/ImageMerger.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Railway_Management.Utilities
{
    // Requires NuGet: SixLabors.ImageSharp
    public static class ImageMerger
    {
        /// <summary>
        /// Merge multiple images into a single grid image. Returns path to saved merged image.
        /// </summary>
        public static async Task<string> MergeImagesAsync(IEnumerable<string> imagePaths, string outputPath, int columns = 1, int spacing = 0, CancellationToken cancellationToken = default)
        {
            if (imagePaths == null) throw new ArgumentNullException(nameof(imagePaths));
            var list = imagePaths.Where(File.Exists).ToList();
            if (!list.Any()) throw new FileNotFoundException("No source images found");

            // Load all images (if many, consider streaming in chunks)
            var images = new List<Image<Rgba32>>();
            try
            {
                foreach (var p in list)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var fs = File.OpenRead(p);
                    var img = await Image.LoadAsync<Rgba32>(fs, cancellationToken).ConfigureAwait(false);
                    fs.Dispose();
                    images.Add(img);
                }

                columns = Math.Max(1, columns);
                int rows = (int)Math.Ceiling(images.Count / (double)columns);

                int maxWidth = images.Max(i => i.Width);
                int maxHeight = images.Max(i => i.Height);

                int totalWidth = columns * maxWidth + Math.Max(0, columns - 1) * spacing;
                int totalHeight = rows * maxHeight + Math.Max(0, rows - 1) * spacing;

                using var canvas = new Image<Rgba32>(totalWidth, totalHeight);
                canvas.Mutate(ctx => ctx.Fill(Rgba32.White));

                for (int idx = 0; idx < images.Count; idx++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    int col = idx % columns;
                    int row = idx / columns;
                    int x = col * (maxWidth + spacing);
                    int y = row * (maxHeight + spacing);

                    // If sizes differ, center the image in its cell
                    var img = images[idx];
                    int offsetX = x + (maxWidth - img.Width) / 2;
                    int offsetY = y + (maxHeight - img.Height) / 2;

                    canvas.Mutate(ctx => ctx.DrawImage(img, new Point(offsetX, offsetY), 1f));
                }

                Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? string.Empty);
                await canvas.SaveAsPngAsync(outputPath, cancellationToken).ConfigureAwait(false);
                return outputPath;
            }
            finally
            {
                foreach (var i in images) i.Dispose();
            }
        }
    }
}