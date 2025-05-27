using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SevenZipSharpArchiver.Api.Examples
{
    /// <summary>
    /// Contains examples of using the ArchiveApi
    /// </summary>
    public class ApiExample
    {
        /// <summary>
        /// Shows how to use the API to compress a single file
        /// </summary>
        public static void CompressFileExample()
        {
            // Create API instance
            var api = new ArchiveApi();
            
            // Define input and output paths
            string inputFile = @"C:\temp\document.txt";
            string outputFile = @"C:\temp\document.7z";
            
            // Compress the file
            ArchiveResult result = api.CompressFile(inputFile, outputFile, "text");
            
            // Check the result
            if (result.Success)
            {
                Console.WriteLine($"File compressed successfully: {outputFile}");
            }
            else
            {
                Console.WriteLine($"Compression failed: {result.Message}");
                if (result.Exception != null)
                {
                    Console.WriteLine($"Exception: {result.Exception.Message}");
                }
            }
        }
        
        /// <summary>
        /// Shows how to use the API to compress multiple files
        /// </summary>
        public static void CompressMultipleFilesExample()
        {
            // Create API instance
            var api = new ArchiveApi();
            
            // Define input files and output path
            var inputFiles = new List<string>
            {
                @"C:\temp\file1.txt",
                @"C:\temp\file2.txt",
                @"C:\temp\file3.txt"
            };
            string outputFile = @"C:\temp\archive.7z";
            
            // Compress the files
            ArchiveResult result = api.CompressFiles(inputFiles, outputFile, "logs");
            
            // Check the result
            if (result.Success)
            {
                Console.WriteLine($"Files compressed successfully: {outputFile}");
            }
            else
            {
                Console.WriteLine($"Compression failed: {result.Message}");
            }
        }
        
        /// <summary>
        /// Shows how to use the API to decompress an archive
        /// </summary>
        public static void DecompressArchiveExample()
        {
            // Create API instance
            var api = new ArchiveApi();
            
            // Define input and output paths
            string archiveFile = @"C:\temp\archive.7z";
            string outputDirectory = @"C:\temp\extracted";
            
            // Create output directory if it doesn't exist
            Directory.CreateDirectory(outputDirectory);
            
            // Decompress the archive
            ArchiveResult result = api.DecompressArchive(archiveFile, outputDirectory);
            
            // Check the result
            if (result.Success)
            {
                Console.WriteLine($"Archive decompressed successfully to: {outputDirectory}");
            }
            else
            {
                Console.WriteLine($"Decompression failed: {result.Message}");
            }
        }
        
        /// <summary>
        /// Shows how to use the API asynchronously
        /// </summary>
        public static async Task AsyncExampleAsync()
        {
            // Create API instance
            var api = new ArchiveApi();
            
            // Define input and output paths
            string inputFile = @"C:\temp\document.txt";
            string outputFile = @"C:\temp\document.7z";
            
            // Compress the file asynchronously
            Console.WriteLine("Starting asynchronous compression...");
            ArchiveResult result = await api.CompressFileAsync(inputFile, outputFile, "text");
            
            // Check the result
            if (result.Success)
            {
                Console.WriteLine($"File compressed successfully: {outputFile}");
            }
            else
            {
                Console.WriteLine($"Compression failed: {result.Message}");
            }
        }
    }
} 