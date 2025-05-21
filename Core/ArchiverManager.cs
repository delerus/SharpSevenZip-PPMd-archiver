using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZipSharpArchiver.Core.Models;
using SevenZipSharpArchiver.Core.Compression;
using SevenZipSharpArchiver.Core.IO;
using SevenZipSharpArchiver.Core.Configuration;
using SevenZipSharpArchiver.Core.Logging;
using SevenZipSharpArchiver.Core.Mappers;
using SevenZipSharpArchiver.Core.Infrastructure;

namespace SevenZipSharpArchiver.Core
{
    public class ArchiverManager
    {
        private PPMdSettings _settings;
        private DecompressionSettings _decompressionSettings;
        private string _inputFilePath;
        private string _outputFilePath;
        private ICompressionSettingsMapper<PPMdSettings> _settingsMapper;
        private ILogger _logger;
        private ICompressorFactory _compressorFactory;
        private IDecompressorFactory _decompressorFactory;

        string[] archiveExtensions = { ".7z", ".zip", ".rar", ".tar", ".gz", ".bz2", ".xz", ".cab", ".iso" };

        public ArchiverManager(string inputFile, string outputFile)
        {
            _settings = new PPMdSettings();
            _decompressionSettings = new DecompressionSettings();
            _inputFilePath = inputFile;
            _outputFilePath = outputFile;
            _settingsMapper = new PPMdSettingsMapper();
            
            // Initialize logger
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            string logFileName = $"archiver_{DateTime.Now:yyyyMMdd_HHmmss}.log";
            string logFilePath = Path.Combine(logDirectory, logFileName);
            _logger = new FileLogger("ArchiverManager", logFilePath);
            
            // Initialize factories
            _compressorFactory = new DefaultCompressorFactory();
            _decompressorFactory = new DefaultDecompressorFactory();
            
            _logger.Information($"ArchiverManager initialized. Input: {inputFile}, Output: {outputFile}");
        }

        public void Init()
        {
            try
            {
                _logger.Debug("Validating input file...");
                IOHandler.ValidateReadFile(_inputFilePath);

                string fileExtension = Path.GetExtension(_inputFilePath).ToLower();
                _logger.Information($"Processing file with extension: {fileExtension}");

                _logger.Debug("Initializing 7z library...");
                BaseLibraryLoader.InitializeLibrary();

                if (archiveExtensions.Contains(fileExtension))
                {
                    _logger.Information("File is an archive. Starting decompression...");
                    Decompression();
                }
                else
                {
                    _logger.Information("File is not an archive. Starting compression...");
                    _logger.Debug("Validating output file...");
                    IOHandler.ValidateWriteFile(_outputFilePath);
                    Compression();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Initialization error", ex);
            }
        }

        public void Compression()
        {
            _logger.Debug("Mapping compression settings to parameters...");
            var compressionParams = _settingsMapper.MapToParameters(_settings);
            var compressor = new Compressor(compressionParams, _settings, _logger, _compressorFactory);
            
            _logger.Information($"Starting compression from {_inputFilePath} to {_outputFilePath}");

            try
            {
                compressor.CompressFile(_inputFilePath, _outputFilePath);
                _logger.Information("Compression completed successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("Compression error", ex);
            }
        }

        public void Decompression()
        {
            using var decompressor = new Decompressor(_decompressionSettings, _logger, _decompressorFactory);

            _logger.Information($"Starting decompression from {_inputFilePath} to {_outputFilePath}");

            try
            {
                decompressor.DecompressFile(_inputFilePath, _outputFilePath);
                _logger.Information("Decompression completed successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("Decompression error", ex);
            }
        }
    }
}
