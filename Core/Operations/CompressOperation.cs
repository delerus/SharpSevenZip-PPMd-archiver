using System;
using System.Collections.Generic;
using System.Linq;
using SevenZipSharpArchiver.Core.Compression;
using SevenZipSharpArchiver.Core.Logging;
using SevenZipSharpArchiver.Core.IO;
using SevenZipSharpArchiver.Core.Models;
using SevenZipSharpArchiver.Core.Mappers;
using SevenZipSharpArchiver.Core.Profiling;

namespace SevenZipSharpArchiver.Core.Operations
{
    /// <summary>
    /// Operation for compressing files
    /// </summary>
    public class CompressOperation : IOperation
    {
        private readonly ILogger _logger;
        private readonly ICompressorFactory _compressorFactory;
        private readonly string _profileName;
        private readonly ProfileSelector _profileSelector;
        private readonly ICompressionSettingsMapper<PPMdSettings> _settingsMapper;
        
        /// <summary>
        /// Creates a new instance of CompressOperation
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="compressorFactory">Factory for creating compressors</param>
        /// <param name="profileName">Compression profile name</param>
        /// <param name="profileSelector">Profile selector</param>
        /// <param name="settingsMapper">Settings mapper</param>
        public CompressOperation(
            ILogger logger,
            ICompressorFactory compressorFactory = null,
            string profileName = null,
            ProfileSelector profileSelector = null,
            ICompressionSettingsMapper<PPMdSettings> settingsMapper = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _compressorFactory = compressorFactory ?? new DefaultCompressorFactory();
            _profileName = profileName;
            _profileSelector = profileSelector ?? new ProfileSelector(_logger);
            _settingsMapper = settingsMapper ?? new PPMdSettingsMapper();
        }
        
        /// <summary>
        /// Executes the compression operation
        /// </summary>
        public bool Execute(IEnumerable<string> inputFiles, string outputPath)
        {
            try
            {
                var inputFilesList = inputFiles.ToList();
                
                if (inputFilesList.Count == 0)
                {
                    _logger.Error("No input files specified");
                    return false;
                }
                
                // Validate input files
                foreach (var inputFile in inputFilesList)
                {
                    FilePathValidator.ValidateReadFile(inputFile);
                }
                
                // Validate output file
                FilePathValidator.ValidateWriteFile(outputPath);
                
                // Apply profile to settings
                string primaryFile = inputFilesList.First();
                _logger.Debug($"Applying compression profile based on file: {primaryFile}");
                var settings = new PPMdSettings();
                settings = _profileSelector.SelectAndApplyProfile(settings, primaryFile, _profileName);
                
                // Map settings to parameters
                _logger.Debug("Mapping compression settings to parameters...");
                var compressionParams = _settingsMapper.MapToParameters(settings);
                
                // Create compressor
                var compressor = new Compressor(compressionParams, settings, _logger, _compressorFactory);
                
                // Execute compression
                if (inputFilesList.Count == 1)
                {
                    _logger.Information($"Starting compression of single file from {inputFilesList[0]} to {outputPath}");
                    compressor.CompressFile(inputFilesList[0], outputPath);
                }
                else
                {
                    _logger.Information($"Starting compression of {inputFilesList.Count} files to {outputPath}");
                    compressor.CompressFiles(inputFilesList, outputPath);
                }
                
                _logger.Information("Compression completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Compression error: {ex.Message}", ex);
                return false;
            }
        }
    }
} 