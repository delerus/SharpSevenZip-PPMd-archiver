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
        private List<string> _inputFilePaths;
        private string _outputPath;
        private ICompressionSettingsMapper<PPMdSettings> _settingsMapper;
        private ILogger _logger;
        private ICompressorFactory _compressorFactory;
        private IDecompressorFactory _decompressorFactory;

        string[] archiveExtensions = { ".7z", ".zip", ".rar", ".tar", ".gz", ".bz2", ".xz", ".cab", ".iso" };

        /// <summary>
        /// Creates a new archiver manager for single file operations
        /// </summary>
        /// <param name="inputFile">Input file path</param>
        /// <param name="outputFile">Output file path</param>
        public ArchiverManager(string inputFile, string outputFile)
            : this(new List<string> { inputFile }, outputFile) { }

        /// <summary>
        /// Creates a new archiver manager for multiple file operations
        /// </summary>
        /// <param name="inputFiles">List of input file paths</param>
        /// <param name="outputPath">Output file or directory path</param>
        public ArchiverManager(IEnumerable<string> inputFiles, string outputPath)
        {
            _settings = new PPMdSettings();
            _decompressionSettings = new DecompressionSettings();
            _inputFilePaths = inputFiles?.ToList() ?? throw new ArgumentNullException(nameof(inputFiles));
            _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            _settingsMapper = new PPMdSettingsMapper();
            
            // Initialize logger
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            string logFileName = $"archiver_{DateTime.Now:yyyyMMdd_HHmmss}.log";
            string logFilePath = Path.Combine(logDirectory, logFileName);
            _logger = new FileLogger("ArchiverManager", logFilePath);
            
            // Initialize factories
            _compressorFactory = new DefaultCompressorFactory();
            _decompressorFactory = new DefaultDecompressorFactory();
            
            _logger.Information($"ArchiverManager initialized. Input files: {_inputFilePaths.Count}, Output: {outputPath}");
        }

        public void Init()
        {
            try
            {
                if (_inputFilePaths.Count == 0)
                {
                    _logger.Error("No input files specified");
                    throw new ArgumentException("No input files specified");
                }

                _logger.Debug("Validating input files...");
                foreach (var inputFile in _inputFilePaths)
                {
                    IOHandler.ValidateReadFile(inputFile);
                }

                _logger.Debug("Initializing 7z library...");
                BaseLibraryLoader.InitializeLibrary();

                // For single file operations, check if we're decompressing or compressing
                if (_inputFilePaths.Count == 1)
                {
                    string fileExtension = Path.GetExtension(_inputFilePaths[0]).ToLower();
                    _logger.Information($"Processing file with extension: {fileExtension}");

                    if (archiveExtensions.Contains(fileExtension))
                    {
                        _logger.Information("File is an archive. Starting decompression...");
                        Decompression();
                    }
                    else
                    {
                        _logger.Information("File is not an archive. Starting compression...");
                        _logger.Debug("Validating output file...");
                        IOHandler.ValidateWriteFile(_outputPath);
                        Compression();
                    }
                }
                // For multiple files, always compress
                else
                {
                    _logger.Information($"Processing {_inputFilePaths.Count} files for compression");
                    _logger.Debug("Validating output file...");
                    IOHandler.ValidateWriteFile(_outputPath);
                    Compression();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Initialization error", ex);
                throw;
            }
        }

        /// <summary>
        /// Extracts specific files from an archive
        /// </summary>
        /// <param name="fileNamesToExtract">List of file names to extract from the archive</param>
        public void ExtractSpecificFiles(IEnumerable<string> fileNamesToExtract)
        {
            if (_inputFilePaths.Count != 1)
            {
                _logger.Error("Multiple input archives not supported for selective extraction");
                throw new InvalidOperationException("Multiple input archives not supported for selective extraction");
            }

            string inputArchive = _inputFilePaths[0];
            string fileExtension = Path.GetExtension(inputArchive).ToLower();

            if (!archiveExtensions.Contains(fileExtension))
            {
                _logger.Error($"The file {inputArchive} is not a recognized archive format");
                throw new InvalidOperationException($"The file {inputArchive} is not a recognized archive format");
            }

            _logger.Information($"Extracting specific files from {inputArchive}");
            
            var decompressor = new Decompressor(_decompressionSettings, _logger, _decompressorFactory);
            try
            {
                decompressor.ExtractFiles(inputArchive, fileNamesToExtract, _outputPath);
                _logger.Information("Extraction of specific files completed");
            }
            catch (Exception ex)
            {
                _logger.Error("Error extracting specific files", ex);
                throw;
            }
        }

        public void Compression()
        {
            _logger.Debug("Mapping compression settings to parameters...");
            var compressionParams = _settingsMapper.MapToParameters(_settings);
            var compressor = new Compressor(compressionParams, _settings, _logger, _compressorFactory);
            
            try
            {
                if (_inputFilePaths.Count == 1)
                {
                    _logger.Information($"Starting compression of single file from {_inputFilePaths[0]} to {_outputPath}");
                    compressor.CompressFile(_inputFilePaths[0], _outputPath);
                }
                else
                {
                    _logger.Information($"Starting compression of {_inputFilePaths.Count} files to {_outputPath}");
                    compressor.CompressFiles(_inputFilePaths, _outputPath);
                }
                
                _logger.Information("Compression completed successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("Compression error", ex);
                throw;
            }
        }

        public void Decompression()
        {
            if (_inputFilePaths.Count != 1)
            {
                _logger.Error("Multiple input archives not supported for decompression");
                throw new InvalidOperationException("Multiple input archives not supported for decompression");
            }

            var decompressor = new Decompressor(_decompressionSettings, _logger, _decompressorFactory);

            _logger.Information($"Starting decompression from {_inputFilePaths[0]} to {_outputPath}");

            try
            {
                decompressor.DecompressFile(_inputFilePaths[0], _outputPath);
                _logger.Information("Decompression completed successfully");
            }
            catch (Exception ex)
            {
                _logger.Error("Decompression error", ex);
                throw;
            }
        }
    }
}
