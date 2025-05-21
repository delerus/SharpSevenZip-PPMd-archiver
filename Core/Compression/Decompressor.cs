using System;
using SharpSevenZip;
using SevenZipSharpArchiver.Core.Configuration;
using SevenZipSharpArchiver.Core.Models;
using SevenZipSharpArchiver.Core.Logging;

namespace SevenZipSharpArchiver.Core.Compression
{
    /// <summary>
    /// Decompressor implementation for archive extraction
    /// </summary>
    public class Decompressor : IDecompressor, IDisposable
    {
        private readonly DecompressionSettings _settings;
        private readonly ILogger _logger;
        private readonly IDecompressorFactory _decompressorFactory;
        private SharpSevenZipExtractor _decompressor;
        private bool _disposed = false;

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
                _decompressor = _decompressorFactory.CreateDecompressor(inputFilePath);
                
                _logger.Debug("Configuring decompressor with settings");
                // Configure the decompressor using settings
                _decompressor = DecompressorConfigurator.Configure(_decompressor, _settings);
                
                _logger.Information($"Starting decompression of {inputFilePath} to {outputFilePath}");
                _decompressor.ExtractArchive(outputFilePath);
                _logger.Information($"Successfully decompressed archive to {outputFilePath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error decompressing file {inputFilePath}", ex);
                throw new Exception($"Error decompressing file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Disposes the decompressor resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the decompressor resources
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if from finalizer</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing && _decompressor != null)
            {
                _logger?.Debug("Disposing decompressor resources");
                _decompressor.Dispose();
                _decompressor = null;
            }

            _disposed = true;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Decompressor()
        {
            Dispose(false);
        }
    }
}
