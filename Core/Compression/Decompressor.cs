using System;
using System.Collections.Generic;
using System.Linq;
using SharpSevenZip;
using SevenZipSharpArchiver.Core.Configuration;
using SevenZipSharpArchiver.Core.Models;
using SevenZipSharpArchiver.Core.Logging;

namespace SevenZipSharpArchiver.Core.Compression
{
    /// <summary>
    /// Decompressor implementation for archive extraction
    /// </summary>
    public class Decompressor : IDecompressor
    {
        private readonly DecompressionSettings _settings;
        private readonly ILogger _logger;
        private readonly IDecompressorFactory _decompressorFactory;

        /// <summary>
        /// Creates a new instance of decompressor with specified settings and dependencies
        /// </summary>
        /// <param name="settings">Decompression settings</param>
        /// <param name="logger">Logger instance</param>
        /// <param name="decompressorFactory">Factory for creating SharpSevenZipExtractor instances</param>
        public Decompressor(
            DecompressionSettings settings,
            ILogger logger,
            IDecompressorFactory decompressorFactory = null)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _decompressorFactory = decompressorFactory ?? new DefaultDecompressorFactory();
        }

        /// <summary>
        /// Decompresses an archive file
        /// </summary>
        /// <param name="inputFilePath">Input archive path</param>
        /// <param name="outputFilePath">Output directory path</param>
        public void DecompressFile(string inputFilePath, string outputFilePath)
        {
            try
            {
                _logger.Debug($"Creating decompressor for {inputFilePath}");
                var decompressor = _decompressorFactory.CreateDecompressor(inputFilePath);
                
                _logger.Debug("Configuring decompressor with settings");
                // Configure the decompressor using settings
                var configuredDecompressor = DecompressorConfigurator.Configure(decompressor, _settings);
                
                _logger.Information($"Starting decompression of {inputFilePath} to {outputFilePath}");
                configuredDecompressor.ExtractArchive(outputFilePath);
                _logger.Information($"Successfully decompressed archive to {outputFilePath}");
                
                decompressor.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error decompressing file {inputFilePath}", ex);
                throw new Exception($"Error decompressing file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Extracts specific files from an archive
        /// </summary>
        /// <param name="inputFilePath">Input archive path</param>
        /// <param name="fileNamesToExtract">List of file names to extract</param>
        /// <param name="outputFilePath">Output directory path</param>
        public void ExtractFiles(string inputFilePath, IEnumerable<string> fileNamesToExtract, string outputFilePath)
        {
            try
            {
                if (fileNamesToExtract == null || !fileNamesToExtract.Any())
                {
                    _logger.Warning("No specific files provided for extraction. Extracting entire archive.");
                    DecompressFile(inputFilePath, outputFilePath);
                    return;
                }

                _logger.Debug($"Creating decompressor for extracting specific files from {inputFilePath}");
                var decompressor = _decompressorFactory.CreateDecompressor(inputFilePath);
                
                _logger.Debug("Configuring decompressor with settings");
                // Configure the decompressor using settings
                var configuredDecompressor = DecompressorConfigurator.Configure(decompressor, _settings);

                // Get all files in the archive
                var archiveFiles = configuredDecompressor.ArchiveFileData;
                
                // Find the indexes of files to extract
                var filesToExtract = new List<int>();
                foreach (var fileName in fileNamesToExtract)
                {
                    for (int i = 0; i < archiveFiles.Count; i++)
                    {
                        if (archiveFiles[i].FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                        {
                            filesToExtract.Add(i);
                            break;
                        }
                    }
                }

                if (filesToExtract.Count == 0)
                {
                    _logger.Warning("None of the specified files found in the archive.");
                    return;
                }

                _logger.Information($"Starting extraction of {filesToExtract.Count} files from {inputFilePath} to {outputFilePath}");
                configuredDecompressor.ExtractFiles(outputFilePath, filesToExtract.ToArray());
                _logger.Information($"Successfully extracted {filesToExtract.Count} files to {outputFilePath}");
                
                decompressor.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error extracting files from {inputFilePath}", ex);
                throw new Exception($"Error extracting files: {ex.Message}", ex);
            }
        }
    }
}
