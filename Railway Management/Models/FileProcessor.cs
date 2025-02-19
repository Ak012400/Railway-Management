using System.IO.Compression;
using ZstdSharp;
//using ZstdNet; // Zstandard Library
using SevenZipExtractor;
using SevenZip; // LZMA Library

namespace Railway_Management.Models
{
    public class FileProcessor
    {
        public static byte[] CompressFile(byte[] file)
        {
            using var compressor = new Compressor();
            return compressor.Wrap(file).ToArray();
        }
        public static byte[] Decompress(byte[] file)
        {
            using var Decompressor = new Decompressor();
            return Decompressor.Unwrap(file).ToArray();
        }
        public static byte[] GetFileByte(string filePath)
        {
            byte[] filebyte = File.ReadAllBytes(filePath);
            return filebyte;
        }

        public static void CheckDifferentAlgos(string inputFile)
        {
            string gzipFile = inputFile + ".gz";
            string deflateFile = inputFile + ".deflate";
            string brotliFile = inputFile + ".br";
            string zstdFile = inputFile + ".zst";
            string lzmaFile = inputFile + ".7z";
            try
            {


                CompressFile(inputFile, gzipFile, System.IO.Compression.CompressionLevel.Optimal, CompressionType.Gzip);
                CompressFile(inputFile, deflateFile, System.IO.Compression.CompressionLevel.Optimal, CompressionType.Deflate);
                CompressFile(inputFile, brotliFile, System.IO.Compression.CompressionLevel.Optimal, CompressionType.Brotli);
                CompressWithZstd(inputFile, zstdFile, 22); // Max compression level 22
                CompressWithLzma(inputFile, lzmaFile);

                Console.WriteLine("Original File Size: " + new FileInfo(inputFile).Length + " bytes");
                Console.WriteLine("Gzip Compressed Size: " + new FileInfo(gzipFile).Length + " bytes");
                Console.WriteLine("Deflate Compressed Size: " + new FileInfo(deflateFile).Length + " bytes");
                Console.WriteLine("Brotli Compressed Size: " + new FileInfo(brotliFile).Length + " bytes");
                Console.WriteLine("Zstd Compressed Size: " + new FileInfo(zstdFile).Length + " bytes");
                Console.WriteLine("LZMA Compressed Size: " + new FileInfo(lzmaFile).Length + " bytes");
            }catch (Exception ex) { Console.WriteLine(ex.Message); }    
        }

        enum CompressionType { Gzip, Deflate, Brotli }

        private static void CompressFile(string inputFile, string outputFile, System.IO.Compression.CompressionLevel level, CompressionType type)
        {
            try
            {


                using (FileStream inputStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                using (FileStream outputStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                {
                    Stream compressionStream = type switch
                    {
                        CompressionType.Gzip => new GZipStream(outputStream, level),
                        CompressionType.Deflate => new DeflateStream(outputStream, level),
                        CompressionType.Brotli => new BrotliStream(outputStream, level),
                        _ => throw new NotImplementedException()
                    };

                    inputStream.CopyTo(compressionStream);
                    compressionStream.Close();
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void CompressWithZstd(string inputFile, string outputFile, int compressionLevel)
        {
            try
            {
                byte[] inputBytes = File.ReadAllBytes(inputFile);
                using (var compressor = new Compressor())
                {
                    byte[] compressedData = compressor.Wrap(inputBytes).ToArray();
                    File.WriteAllBytes(outputFile, compressedData);
                }
            }catch(Exception ex) { Console.WriteLine(ex.Message.ToString()); }  
        }

        private static void CompressWithLzma(string inputFile, string outputFile)
        {
            try
            {


                SevenZipCompressor archive = new SevenZipCompressor();
                archive.CompressionLevel = (SevenZip.CompressionLevel)System.IO.Compression.CompressionLevel.SmallestSize;
                archive.CompressFiles(outputFile, inputFile);
            }catch(Exception ex) { Console.WriteLine(ex.ToString() ); } 
        }
    }
}
