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
        
        /// <summary>
        /// Shows how to use automatic operation detection
        /// </summary>
        public static void AutomaticOperationExample()
        {
            // Create API instance
            var api = new ArchiveApi();
            
            // Example 1: Compression (detected by output file extension)
            Console.WriteLine("Example 1: Automatic compression");
            var inputFiles = new List<string>
            {
                @"C:\temp\file1.txt",
                @"C:\temp\file2.txt"
            };
            string outputArchive = @"C:\temp\auto_detected.7z";
            
            ArchiveResult compressionResult = api.ProcessAutomatic(inputFiles, outputArchive);
            
            if (compressionResult.Success)
            {
                Console.WriteLine($"Automatic compression succeeded: {outputArchive}");
            }
            else
            {
                Console.WriteLine($"Automatic compression failed: {compressionResult.Message}");
            }
            
            // Example 2: Decompression (detected by input file being an archive)
            Console.WriteLine("\nExample 2: Automatic decompression");
            var archiveFile = new List<string> { @"C:\temp\archive_to_extract.7z" };
            string extractPath = @"C:\temp\auto_extracted";
            
            // Ensure extract directory exists
            Directory.CreateDirectory(extractPath);
            
            ArchiveResult decompressionResult = api.ProcessAutomatic(archiveFile, extractPath);
            
            if (decompressionResult.Success)
            {
                Console.WriteLine($"Automatic decompression succeeded: {extractPath}");
            }
            else
            {
                Console.WriteLine($"Automatic decompression failed: {decompressionResult.Message}");
            }
        }
        
        /// <summary>
        /// Shows how to use automatic operation detection asynchronously
        /// </summary>
        public static async Task AutomaticOperationAsyncExample()
        {
            // Create API instance
            var api = new ArchiveApi();
            
            // Define input files and output path
            var inputFiles = new List<string> { @"C:\temp\large_log_file.txt" };
            string outputFile = @"C:\temp\auto_detected_async.7z";
            
            Console.WriteLine("Starting automatic operation (async)...");
            
            // Let the API automatically detect and execute the appropriate operation
            ArchiveResult result = await api.ProcessAutomaticAsync(inputFiles, outputFile);
            
            if (result.Success)
            {
                Console.WriteLine($"Automatic operation completed successfully");
            }
            else
            {
                Console.WriteLine($"Automatic operation failed: {result.Message}");
            }
        }
        
        /// <summary>
        /// Shows how to use the API to decompress multiple archives at once
        /// </summary>
        public static void DecompressMultipleArchivesExample()
        {
            // Create API instance
            var api = new ArchiveApi();
            
            // Define input archives and output directory
            var archiveFiles = new List<string>
            {
                @"C:\temp\archive1.7z",
                @"C:\temp\archive2.zip",
                @"C:\temp\archive3.rar"
            };
            string outputDirectory = @"C:\temp\extracted_multiple";
            
            // Create output directory if it doesn't exist
            Directory.CreateDirectory(outputDirectory);
            
            // Decompress the archives
            Console.WriteLine($"Decompressing {archiveFiles.Count} archives...");
            ArchiveResult result = api.DecompressArchives(archiveFiles, outputDirectory);
            
            // Check the result
            if (result.Success)
            {
                Console.WriteLine($"All archives decompressed successfully to: {outputDirectory}");
                Console.WriteLine("Each archive was extracted to its own subdirectory.");
            }
            else
            {
                Console.WriteLine($"Decompression failed: {result.Message}");
                if (result.Exception != null)
                {
                    Console.WriteLine($"Exception: {result.Exception.Message}");
                }
            }
        }
        
        /// <summary>
        /// Shows how to use automatic operation detection with multiple archives
        /// </summary>
        public static void AutomaticMultipleArchivesExample()
        {
            // Create API instance
            var api = new ArchiveApi();
            
            // Define input archives and output directory
            var archiveFiles = new List<string>
            {
                @"C:\temp\archive1.7z",
                @"C:\temp\archive2.zip"
            };
            string outputDirectory = @"C:\temp\auto_extracted_multiple";
            
            // Create output directory if it doesn't exist
            Directory.CreateDirectory(outputDirectory);
            
            // Let the API automatically detect and execute the appropriate operation
            Console.WriteLine($"Processing {archiveFiles.Count} archives automatically...");
            ArchiveResult result = api.ProcessAutomatic(archiveFiles, outputDirectory);
            
            // Check the result
            if (result.Success)
            {
                Console.WriteLine($"All archives processed successfully to: {outputDirectory}");
            }
            else
            {
                Console.WriteLine($"Processing failed: {result.Message}");
            }
        }
    }
} 