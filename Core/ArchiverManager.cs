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
using SevenZipSharpArchiver.Core.Profiling;
using SevenZipSharpArchiver.Core.Operations;

namespace SevenZipSharpArchiver.Core
{
    /// <summary>
    /// Main manager class for archiving operations
    /// </summary>
    public class ArchiverManager
    {
        private readonly List<string> _inputFilePaths;
        private readonly string _outputPath;
        private readonly string _profileName;
        private readonly ILogger _logger;
        private readonly IOperationDetector _operationDetector;
        private readonly IOperationFactory _operationFactory;
        
        /// <summary>
        /// Creates a new archiver manager for single file operations
        /// </summary>
        /// <param name="inputFile">Input file path</param>
        /// <param name="outputFile">Output file path</param>
        /// <param name="profileName">Optional compression profile name</param>
        public ArchiverManager(string inputFile, string outputFile, string profileName = null)
            : this(new List<string> { inputFile }, outputFile, profileName) { }
            
        /// <summary>
        /// Creates a new archiver manager for multiple file operations
        /// </summary>
        /// <param name="inputFiles">List of input file paths</param>
        /// <param name="outputPath">Output file or directory path</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <param name="logger">Logger instance</param>
        /// <param name="operationDetector">Operation detector</param>
        /// <param name="operationFactory">Operation factory</param>
        public ArchiverManager(
            IEnumerable<string> inputFiles, 
            string outputPath, 
            string profileName = null,
            ILogger logger = null,
            IOperationDetector operationDetector = null,
            IOperationFactory operationFactory = null)
        {
            _inputFilePaths = inputFiles?.ToList() ?? throw new ArgumentNullException(nameof(inputFiles));
            _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            _profileName = profileName;
            
            // Initialize logger if not provided
            if (logger == null)
            {
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                string logFileName = $"archiver_{DateTime.Now:yyyyMMdd_HHmmss}.log";
                string logFilePath = Path.Combine(logDirectory, logFileName);
                _logger = new FileLogger("ArchiverManager", logFilePath);
            }
            else
            {
                _logger = logger;
            }
            
            // Initialize operation detector if not provided
            _operationDetector = operationDetector ?? new DefaultOperationDetector(_logger);
            
            // Initialize operation factory if not provided
            if (operationFactory == null)
            {
                // First initialize 7z library
                BaseLibraryLoader.InitializeLibrary();
                _operationFactory = new DefaultOperationFactory(_logger);
            }
            else
            {
                _operationFactory = operationFactory;
            }
            
            _logger.Information($"ArchiverManager initialized. Input files: {_inputFilePaths.Count}, Output: {outputPath}, Profile: {_profileName ?? "auto"}");
        }
        
        /// <summary>
        /// Initializes and executes the appropriate operation
        /// </summary>
        public void Init()
        {
            try
            {
                if (_inputFilePaths.Count == 0)
                {
                    _logger.Error("No input files specified");
                    throw new ArgumentException("No input files specified");
                }
                
                // Detect operation type
                OperationType operationType = _operationDetector.DetectOperation(_inputFilePaths, _outputPath);
                
                // Create operation
                IOperation operation = _operationFactory.CreateOperation(operationType, _profileName);
                
                // Execute operation
                bool success = operation.Execute(_inputFilePaths, _outputPath);
                
                if (!success)
                {
                    throw new InvalidOperationException("Operation failed");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Operation error", ex);
                throw;
            }
        }
    }
}
