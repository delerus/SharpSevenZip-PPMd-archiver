using System;
using System.Collections.Generic;
using System.Linq;
using SevenZipSharpArchiver.Core.Compression;
using SevenZipSharpArchiver.Core.Logging;
using SevenZipSharpArchiver.Core.IO;
using SevenZipSharpArchiver.Core.Models;

namespace SevenZipSharpArchiver.Core.Operations
{
    /// <summary>
    /// Operation for decompressing archives
    /// </summary>
    public class DecompressOperation : IOperation
    {
        private readonly ILogger _logger;
        private readonly IDecompressorFactory _decompressorFactory;
        private readonly DecompressionSettings _decompressionSettings;
        
        /// <summary>
        /// Creates a new instance of DecompressOperation
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="decompressorFactory">Factory for creating decompressors</param>
        /// <param name="decompressionSettings">Decompression settings</param>
        public DecompressOperation(
            ILogger logger,
            IDecompressorFactory decompressorFactory = null,
            DecompressionSettings decompressionSettings = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _decompressorFactory = decompressorFactory ?? new DefaultDecompressorFactory();
            _decompressionSettings = decompressionSettings ?? new DecompressionSettings();
        }
        
        /// <summary>
        /// Executes the decompression operation
        /// </summary>
        public bool Execute(IEnumerable<string> inputFiles, string outputPath)
        {
            try
            {
                var inputFilesList = inputFiles.ToList();
                
                if (inputFilesList.Count != 1)
                {
                    _logger.Error("Multiple input archives not supported for decompression");
                    return false;
                }
                
                string inputFile = inputFilesList.First();
                
                // Validate input file
                FilePathValidator.ValidateReadFile(inputFile);
                
                // Create decompressor
                var decompressor = new Decompressor(_decompressionSettings, _logger, _decompressorFactory);
                
                // Execute decompression
                _logger.Information($"Starting decompression from {inputFile} to {outputPath}");
                decompressor.DecompressFile(inputFile, outputPath);
                
                _logger.Information("Decompression completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Decompression error: {ex.Message}", ex);
                return false;
            }
        }
    }
} 