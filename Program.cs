using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SevenZipSharpArchiver.Core;
using SevenZipSharpArchiver.Core.Logging;
using SevenZipSharpArchiver.Core.Operations;

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
                // Parse command line arguments
                var (profileName, inputFiles, outputPath) = ParseCommandLineArgs(args);
                
                if (inputFiles.Count == 0 || outputPath == null)
                {
                    ShowUsage();
                    Console.ReadKey();
                    return;
                }
                
                Console.WriteLine($"Processing {inputFiles.Count} input file(s)");
                
                // Create logger
                var logger = CreateConsoleLogger();
                
                // Create archiver manager using builder
                var archiver = new ArchiverManagerBuilder()
                    .WithInputFiles(inputFiles)
                    .WithOutputPath(outputPath)
                    .WithProfile(profileName)
                    .WithLoggerFactory(new DefaultLoggerFactory())
                    .Build();
                
                // Execute operation
                archiver.Execute();
                
                Console.WriteLine("Operation completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Parses command line arguments
        /// </summary>
        static (string profileName, List<string> inputFiles, string outputPath) ParseCommandLineArgs(string[] args)
        {
            string profileName = null;
            List<string> inputFiles = new List<string>();
            string outputPath = null;
            
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                
                // Check for profile flag
                if (arg.StartsWith("--profile=", StringComparison.OrdinalIgnoreCase))
                {
                    profileName = arg.Substring("--profile=".Length);
                    Console.WriteLine($"Using compression profile: {profileName}");
                    continue;
                }
                
                // If it's the last argument and we haven't set outputPath
                if (i == args.Length - 1 && outputPath == null)
                {
                    outputPath = arg;
                }
                else
                {
                    inputFiles.Add(arg);
                }
            }
            
            return (profileName, inputFiles, outputPath);
        }
        
        /// <summary>
        /// Creates a console logger
        /// </summary>
        static ILogger CreateConsoleLogger()
        {
            // For simplicity, we're using a file logger that also logs to console
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logDirectory);
            string logFileName = $"archiver_{DateTime.Now:yyyyMMdd_HHmmss}.log";
            string logFilePath = Path.Combine(logDirectory, logFileName);
            return new FileLogger("ArchiverManager", logFilePath);
        }
        
        static void ShowUsage()
        {
            Console.WriteLine("7zSharpArchiver - PPMd Archiver using SharpSevenZip");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  Compression (single file):");
            Console.WriteLine("    7zSharpArchiver.exe inputFile outputArchive.7z");
            Console.WriteLine("    7zSharpArchiver.exe --profile=logs inputFile outputArchive.7z");
            Console.WriteLine();
            Console.WriteLine("  Compression (multiple files):");
            Console.WriteLine("    7zSharpArchiver.exe file1.txt file2.txt file3.txt outputArchive.7z");
            Console.WriteLine("    7zSharpArchiver.exe --profile=text file1.txt file2.txt outputArchive.7z");
            Console.WriteLine();
            Console.WriteLine("  Decompression:");
            Console.WriteLine("    7zSharpArchiver.exe archive.7z outputDirectory");
            Console.WriteLine();
            Console.WriteLine("  Supported profiles:");
            Console.WriteLine("    --profile=logs   - Optimized for log files, JSON, XML and other structured data");
            Console.WriteLine("    --profile=text   - Standard profile for text documents (default)");
            Console.WriteLine("    --profile=extreme - Maximum compression (slower)");
            Console.WriteLine();
            Console.WriteLine("Notes:");
            Console.WriteLine("  - For compression: The last argument is always the output archive path");
            Console.WriteLine("  - For decompression: The input must be a single archive file");
            Console.WriteLine("  - If no profile is specified, it will be selected automatically based on file type");
            Console.WriteLine("  - Supported archive formats: .7z, .zip, .rar, .tar, .gz, .bz2, .xz, .cab, .iso");
        }
    }
}