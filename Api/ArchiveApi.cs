using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using SevenZipSharpArchiver.Core;
using SevenZipSharpArchiver.Core.Logging;
using SevenZipSharpArchiver.Core.Operations;

namespace SevenZipSharpArchiver.Api
{
    /// <summary>
    /// Public API for integrating 7zSharpArchiver functionality into other applications
    /// </summary>
    public class ArchiveApi
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private readonly IOperationDetector _operationDetector;
        
        /// <summary>
        /// Creates a new instance of ArchiveApi with default logging
        /// </summary>
        public ArchiveApi()
            : this(new DefaultLoggerFactory())
        {
        }
        
        /// <summary>
        /// Creates a new instance of ArchiveApi with custom logger factory
        /// </summary>
        /// <param name="loggerFactory">Logger factory to use</param>
        public ArchiveApi(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = _loggerFactory.CreateLogger(nameof(ArchiveApi));
            _operationDetector = new DefaultOperationDetector(_logger);
        }
        
        /// <summary>
        /// Automatically determines operation type and processes files accordingly
        /// </summary>
        /// <param name="inputFiles">Input files to process</param>
        /// <param name="outputPath">Output path (file or directory)</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <returns>Result of the operation</returns>
        public ArchiveResult ProcessAutomatic(IEnumerable<string> inputFiles, string outputPath, string profileName = null)
        {
            try
            {
                _logger.Information($"API: Auto-detecting operation for {outputPath}");
                
                // Detect operation type
                var operationType = _operationDetector.DetectOperation(inputFiles, outputPath);
                
                // Process based on detected operation type
                if (operationType == OperationType.Decompress)
                {
                    _logger.Information($"API: Detected decompression operation");
                    
                    // Check if we have multiple archives
                    var inputFilesList = inputFiles.ToList();
                    if (inputFilesList.Count > 1)
                    {
                        return DecompressArchives(inputFilesList, outputPath);
                    }
                    else
                    {
                        // For single archive decompression
                        var inputFile = inputFilesList.First();
                        return DecompressArchive(inputFile, outputPath);
                    }
                }
                else
                {
                    _logger.Information($"API: Detected compression operation");
                    return CompressFiles(inputFiles, outputPath, profileName);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("API: Auto-detection processing failed", ex);
                return new ArchiveResult
                {
                    Success = false,
                    Message = $"Operation failed: {ex.Message}",
                    Exception = ex
                };
            }
        }
        
        /// <summary>
        /// Asynchronously processes files with automatic operation detection
        /// </summary>
        /// <param name="inputFiles">Input files to process</param>
        /// <param name="outputPath">Output path (file or directory)</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <returns>Result of the operation</returns>
        public Task<ArchiveResult> ProcessAutomaticAsync(IEnumerable<string> inputFiles, string outputPath, string profileName = null)
        {
            return Task.Run(() => ProcessAutomatic(inputFiles, outputPath, profileName));
        }
        
        /// <summary>
        /// Compresses a single file to an archive
        /// </summary>
        /// <param name="inputFile">Path to the input file</param>
        /// <param name="outputFile">Path to the output archive</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <returns>Result of the operation</returns>
        public ArchiveResult CompressFile(string inputFile, string outputFile, string profileName = null)
        {
            try
            {
                _logger.Information($"API: Compressing file {inputFile} to {outputFile}");
                
                var archiver = new ArchiverManagerBuilder()
                    .WithInputFile(inputFile)
                    .WithOutputPath(outputFile)
                    .WithProfile(profileName)
                    .WithLoggerFactory(_loggerFactory)
                    .Build();
                
                archiver.Execute();
                
                return new ArchiveResult
                {
                    Success = true,
                    Message = "File compressed successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.Error("API: Compression failed", ex);
                return new ArchiveResult
                {
                    Success = false,
                    Message = $"Compression failed: {ex.Message}",
                    Exception = ex
                };
            }
        }
        
        /// <summary>
        /// Compresses multiple files to an archive
        /// </summary>
        /// <param name="inputFiles">Paths to the input files</param>
        /// <param name="outputFile">Path to the output archive</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <returns>Result of the operation</returns>
        public ArchiveResult CompressFiles(IEnumerable<string> inputFiles, string outputFile, string profileName = null)
        {
            try
            {
                _logger.Information($"API: Compressing multiple files to {outputFile}");
                
                var archiver = new ArchiverManagerBuilder()
                    .WithInputFiles(inputFiles)
                    .WithOutputPath(outputFile)
                    .WithProfile(profileName)
                    .WithLoggerFactory(_loggerFactory)
                    .Build();
                
                archiver.Execute();
                
                return new ArchiveResult
                {
                    Success = true,
                    Message = "Files compressed successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.Error("API: Compression failed", ex);
                return new ArchiveResult
                {
                    Success = false,
                    Message = $"Compression failed: {ex.Message}",
                    Exception = ex
                };
            }
        }
        
        /// <summary>
        /// Decompresses an archive to a directory
        /// </summary>
        /// <param name="archiveFile">Path to the archive file</param>
        /// <param name="outputDirectory">Path to the output directory</param>
        /// <returns>Result of the operation</returns>
        public ArchiveResult DecompressArchive(string archiveFile, string outputDirectory)
        {
            try
            {
                _logger.Information($"API: Decompressing {archiveFile} to {outputDirectory}");
                
                var archiver = new ArchiverManagerBuilder()
                    .WithInputFile(archiveFile)
                    .WithOutputPath(outputDirectory)
                    .WithLoggerFactory(_loggerFactory)
                    .Build();
                
                archiver.Execute();
                
                return new ArchiveResult
                {
                    Success = true,
                    Message = "Archive decompressed successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.Error("API: Decompression failed", ex);
                return new ArchiveResult
                {
                    Success = false,
                    Message = $"Decompression failed: {ex.Message}",
                    Exception = ex
                };
            }
        }
        
        /// <summary>
        /// Decompresses multiple archives to a directory
        /// </summary>
        /// <param name="archiveFiles">Paths to the archive files</param>
        /// <param name="outputDirectory">Path to the output directory</param>
        /// <returns>Result of the operation</returns>
        public ArchiveResult DecompressArchives(IEnumerable<string> archiveFiles, string outputDirectory)
        {
            try
            {
                _logger.Information($"API: Decompressing {archiveFiles.Count()} archives to {outputDirectory}");
                
                var archiver = new ArchiverManagerBuilder()
                    .WithInputFiles(archiveFiles)
                    .WithOutputPath(outputDirectory)
                    .WithLoggerFactory(_loggerFactory)
                    .Build();
                
                archiver.Execute();
                
                return new ArchiveResult
                {
                    Success = true,
                    Message = "All archives decompressed successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.Error("API: Multiple decompression failed", ex);
                return new ArchiveResult
                {
                    Success = false,
                    Message = $"Decompression failed: {ex.Message}",
                    Exception = ex
                };
            }
        }
        
        /// <summary>
        /// Asynchronously compresses a single file to an archive
        /// </summary>
        /// <param name="inputFile">Path to the input file</param>
        /// <param name="outputFile">Path to the output archive</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <returns>Result of the operation</returns>
        public Task<ArchiveResult> CompressFileAsync(string inputFile, string outputFile, string profileName = null)
        {
            return Task.Run(() => CompressFile(inputFile, outputFile, profileName));
        }
        
        /// <summary>
        /// Asynchronously compresses multiple files to an archive
        /// </summary>
        /// <param name="inputFiles">Paths to the input files</param>
        /// <param name="outputFile">Path to the output archive</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <returns>Result of the operation</returns>
        public Task<ArchiveResult> CompressFilesAsync(IEnumerable<string> inputFiles, string outputFile, string profileName = null)
        {
            return Task.Run(() => CompressFiles(inputFiles, outputFile, profileName));
        }
        
        /// <summary>
        /// Asynchronously decompresses an archive to a directory
        /// </summary>
        /// <param name="archiveFile">Path to the archive file</param>
        /// <param name="outputDirectory">Path to the output directory</param>
        /// <returns>Result of the operation</returns>
        public Task<ArchiveResult> DecompressArchiveAsync(string archiveFile, string outputDirectory)
        {
            return Task.Run(() => DecompressArchive(archiveFile, outputDirectory));
        }
        
        /// <summary>
        /// Asynchronously decompresses multiple archives to a directory
        /// </summary>
        /// <param name="archiveFiles">Paths to the archive files</param>
        /// <param name="outputDirectory">Path to the output directory</param>
        /// <returns>Result of the operation</returns>
        public Task<ArchiveResult> DecompressArchivesAsync(IEnumerable<string> archiveFiles, string outputDirectory)
        {
            return Task.Run(() => DecompressArchives(archiveFiles, outputDirectory));
        }
    }
} 