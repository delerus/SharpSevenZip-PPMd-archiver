using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
        /// Executes the decompression operation for one or multiple archives
        /// </summary>
        public bool Execute(IEnumerable<string> inputFiles, string outputPath)
        {
            try
            {
                var inputFilesList = inputFiles.ToList();
                
                if (inputFilesList.Count == 0)
                {
                    _logger.Error("No input archives specified for decompression");
                    return false;
                }
                
                bool allSuccessful = true;
                
                foreach (string inputFile in inputFilesList)
                {
                    try
                    {
                        // Validate input file
                        FilePathValidator.ValidateReadFile(inputFile);
                        
                        // Determine output directory for this specific archive
                        string archiveOutputPath = outputPath;

                        // Create decompressor
                        var decompressor = new Decompressor(_decompressionSettings, _logger, _decompressorFactory);
                        
                        // Execute decompression
                        _logger.Information($"Starting decompression from {inputFile} to {archiveOutputPath}");
                        decompressor.DecompressFile(inputFile, archiveOutputPath);
                        
                        _logger.Information($"Successfully decompressed {inputFile}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Error decompressing {inputFile}: {ex.Message}", ex);
                        allSuccessful = false;
                        // Continue with next archive instead of failing completely
                    }
                }
                
                if (allSuccessful)
                {
                    _logger.Information("All archives decompressed successfully");
                }
                else
                {
                    _logger.Warning("Some archives were not decompressed successfully");
                }
                
                return allSuccessful;
            }
            catch (Exception ex)
            {
                _logger.Error($"Decompression operation error: {ex.Message}", ex);
                return false;
            }
        }
    }
} 