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
        private readonly ILibraryInitializer _libraryInitializer;
        
        /// <summary>
        /// Creates a new archiver manager for single file operations
        /// </summary>
        /// <param name="inputFile">Input file path</param>
        /// <param name="outputFile">Output file path</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="operationDetector">Operation detector</param>
        /// <param name="operationFactory">Operation factory</param>
        /// <param name="libraryInitializer">Library initializer</param>
        public ArchiverManager(
            string inputFile, 
            string outputFile, 
            string profileName = null,
            ILoggerFactory loggerFactory = null,
            IOperationDetector operationDetector = null,
            IOperationFactory operationFactory = null,
            ILibraryInitializer libraryInitializer = null)
            : this(
                  new List<string> { inputFile }, 
                  outputFile, 
                  profileName, 
                  loggerFactory, 
                  operationDetector, 
                  operationFactory,
                  libraryInitializer) { }
            
        /// <summary>
        /// Creates a new archiver manager for multiple file operations
        /// </summary>
        /// <param name="inputFiles">List of input file paths</param>
        /// <param name="outputPath">Output file or directory path</param>
        /// <param name="profileName">Optional compression profile name</param>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="operationDetector">Operation detector</param>
        /// <param name="operationFactory">Operation factory</param>
        /// <param name="libraryInitializer">Library initializer</param>
        public ArchiverManager(
            IEnumerable<string> inputFiles, 
            string outputPath, 
            string profileName = null,
            ILoggerFactory loggerFactory = null,
            IOperationDetector operationDetector = null,
            IOperationFactory operationFactory = null,
            ILibraryInitializer libraryInitializer = null)
        {
            _inputFilePaths = inputFiles?.ToList() ?? throw new ArgumentNullException(nameof(inputFiles));
            _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            _profileName = profileName;
            
            // Initialize logger
            loggerFactory ??= new DefaultLoggerFactory();
            _logger = loggerFactory.CreateLogger(nameof(ArchiverManager));
            
            // Initialize library
            _libraryInitializer = libraryInitializer ?? new SevenZipLibraryInitializer(_logger);
            _libraryInitializer.Initialize();
            
            // Initialize operation detector
            _operationDetector = operationDetector ?? new DefaultOperationDetector(_logger);
            
            // Initialize operation factory
            _operationFactory = operationFactory ?? new DefaultOperationFactory(_logger);
            
            _logger.Information($"ArchiverManager initialized. Input files: {_inputFilePaths.Count}, Output: {outputPath}, Profile: {_profileName ?? "auto"}");
        }
        
        /// <summary>
        /// Executes the appropriate operation based on input and output paths
        /// </summary>
        public void Execute()
        {
            ValidateInputs();
            
            try
            {
                // Detect operation type
                OperationType operationType = _operationDetector.DetectOperation(_inputFilePaths, _outputPath);
                
                // Create and execute operation
                ExecuteOperation(operationType);
            }
            catch (Exception ex)
            {
                _logger.Error("Operation error", ex);
                throw;
            }
        }
        
        /// <summary>
        /// Validates input files
        /// </summary>
        private void ValidateInputs()
        {
            if (_inputFilePaths.Count == 0)
            {
                _logger.Error("No input files specified");
                throw new ArgumentException("No input files specified");
            }
        }
        
        /// <summary>
        /// Creates and executes the appropriate operation
        /// </summary>
        /// <param name="operationType">Type of operation to execute</param>
        public void ExecuteOperation(OperationType operationType)
        {
            _logger.Debug($"Creating {operationType} operation");
            IOperation operation = _operationFactory.CreateOperation(operationType, _profileName);
            
            _logger.Information($"Executing {operationType} operation");
            bool success = operation.Execute(_inputFilePaths, _outputPath);
            
            if (!success)
            {
                _logger.Error("Operation failed");
                throw new InvalidOperationException("Operation failed");
            }
            
            _logger.Information($"{operationType} operation completed successfully");
        }
    }
}
