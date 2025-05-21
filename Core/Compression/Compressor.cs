using System;
using System.Collections.Generic;
using SevenZipSharpArchiver.Core.Configuration;
using SevenZipSharpArchiver.Core.Models;
using SevenZipSharpArchiver.Core.Logging;
using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Compression
{
    /// <summary>
    /// PPMd compressor implementation
    /// </summary>
    public class Compressor : ICompressor
    {
        private readonly Dictionary<string, string> _compressionParams;
        private readonly PPMdSettings _settings;
        private readonly ILogger _logger;
        private readonly ICompressorFactory _compressorFactory;

        /// <summary>
        /// Creates a new instance of PPMd compressor
        /// </summary>
        /// <param name="compressionParams">Custom compression parameters</param>
        /// <param name="settings">PPMd settings</param>
        /// <param name="logger">Logger instance</param>
        /// <param name="compressorFactory">Factory for creating SharpSevenZipCompressor instances</param>
        public Compressor(
            Dictionary<string, string> compressionParams, 
            PPMdSettings settings,
            ILogger logger,
            ICompressorFactory compressorFactory = null)
        {
            _compressionParams = compressionParams;
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _compressorFactory = compressorFactory ?? new DefaultCompressorFactory();
        }

        /// <summary>
        /// Compresses a file using PPMd algorithm
        /// </summary>
        /// <param name="inputFilePath">Input file path</param>
        /// <param name="outputFilePath">Output file path</param>
        public void CompressFile(string inputFilePath, string outputFilePath)
        {
            try
            {
                _logger.Debug($"Creating compressor for {inputFilePath} to {outputFilePath}");
                
                var compressor = _compressorFactory.CreateCompressor();
                _logger.Debug("Configuring compressor with settings");
                
                // Configure compressor using settings
                compressor = CompressorConfigurator.Configure(compressor, _settings, _compressionParams);
                
                _logger.Information($"Starting compression of {inputFilePath}");
                compressor.CompressFiles(outputFilePath, inputFilePath);
                _logger.Information($"Successfully compressed file to {outputFilePath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error compressing file {inputFilePath}", ex);
                throw new Exception($"Error compressing file: {ex.Message}", ex);
            }
        }
    }
}
