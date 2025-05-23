using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpSevenZip;
using SevenZipSharpArchiver.Core;

namespace SevenZipSharpArchiver
{
    class PPMdArchiver
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                ShowUsage();
                Console.ReadKey();
                return;
            }

            try
            {
                // Last argument is always the output path
                string outputPath = args[args.Length - 1];
                
                // All arguments except the last one are input files
                string[] inputFiles = args.Take(args.Length - 1).ToArray();
                
                Console.WriteLine($"Processing {inputFiles.Length} input file(s)");
                
                var archiver = new ArchiverManager(inputFiles, outputPath);
                archiver.Init();
                
                Console.WriteLine("Operation completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        static void ShowUsage()
        {
            Console.WriteLine("7zSharpArchiver - PPMd Archiver using SharpSevenZip");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  Compression (single file):");
            Console.WriteLine("    7zSharpArchiver.exe inputFile outputArchive.7z");
            Console.WriteLine();
            Console.WriteLine("  Compression (multiple files):");
            Console.WriteLine("    7zSharpArchiver.exe file1.txt file2.txt file3.txt outputArchive.7z");
            Console.WriteLine();
            Console.WriteLine("  Decompression:");
            Console.WriteLine("    7zSharpArchiver.exe archive.7z outputDirectory");
            Console.WriteLine();
            Console.WriteLine("Notes:");
            Console.WriteLine("  - For compression: The last argument is always the output archive path");
            Console.WriteLine("  - For decompression: The input must be a single archive file");
            Console.WriteLine("  - Supported archive formats: .7z, .zip, .rar, .tar, .gz, .bz2, .xz, .cab, .iso");
        }
    }
}