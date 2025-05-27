using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
} 