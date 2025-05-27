using System;
using System.Collections.Generic;
using SevenZipSharpArchiver.Core.Logging;
using SevenZipSharpArchiver.Core.Operations;
using SevenZipSharpArchiver.Core.Infrastructure;

namespace SevenZipSharpArchiver.Core
{
    /// <summary>
    /// Builder class for creating ArchiverManager instances
    /// </summary>
    public class ArchiverManagerBuilder
    {
        private IEnumerable<string> _inputFiles;
        private string _outputPath;
        private string _profileName;
        private ILoggerFactory _loggerFactory;
        private IOperationDetector _operationDetector;
        private IOperationFactory _operationFactory;
        private ILibraryInitializer _libraryInitializer;
        
        /// <summary>
        /// Sets the input files
        /// </summary>
        /// <param name="inputFiles">Input file paths</param>
        /// <returns>This builder instance</returns>
        public ArchiverManagerBuilder WithInputFiles(IEnumerable<string> inputFiles)
        {
            _inputFiles = inputFiles ?? throw new ArgumentNullException(nameof(inputFiles));
            return this;
        }
        
        /// <summary>
        /// Sets a single input file
        /// </summary>
        /// <param name="inputFile">Input file path</param>
        /// <returns>This builder instance</returns>
        public ArchiverManagerBuilder WithInputFile(string inputFile)
        {
            if (string.IsNullOrEmpty(inputFile))
                throw new ArgumentNullException(nameof(inputFile));
                
            _inputFiles = new List<string> { inputFile };
            return this;
        }
        
        /// <summary>
        /// Sets the output path
        /// </summary>
        /// <param name="outputPath">Output file or directory path</param>
        /// <returns>This builder instance</returns>
        public ArchiverManagerBuilder WithOutputPath(string outputPath)
        {
            _outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            return this;
        }
        
        /// <summary>
        /// Sets the compression profile name
        /// </summary>
        /// <param name="profileName">Profile name</param>
        /// <returns>This builder instance</returns>
        public ArchiverManagerBuilder WithProfile(string profileName)
        {
            _profileName = profileName;
            return this;
        }
        
        /// <summary>
        /// Sets the logger factory
        /// </summary>
        /// <param name="loggerFactory">Logger factory</param>
        /// <returns>This builder instance</returns>
        public ArchiverManagerBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            return this;
        }
        
        /// <summary>
        /// Sets the operation detector
        /// </summary>
        /// <param name="operationDetector">Operation detector</param>
        /// <returns>This builder instance</returns>
        public ArchiverManagerBuilder WithOperationDetector(IOperationDetector operationDetector)
        {
            _operationDetector = operationDetector ?? throw new ArgumentNullException(nameof(operationDetector));
            return this;
        }
        
        /// <summary>
        /// Sets the operation factory
        /// </summary>
        /// <param name="operationFactory">Operation factory</param>
        /// <returns>This builder instance</returns>
        public ArchiverManagerBuilder WithOperationFactory(IOperationFactory operationFactory)
        {
            _operationFactory = operationFactory ?? throw new ArgumentNullException(nameof(operationFactory));
            return this;
        }
        
        /// <summary>
        /// Sets the library initializer
        /// </summary>
        /// <param name="libraryInitializer">Library initializer</param>
        /// <returns>This builder instance</returns>
        public ArchiverManagerBuilder WithLibraryInitializer(ILibraryInitializer libraryInitializer)
        {
            _libraryInitializer = libraryInitializer ?? throw new ArgumentNullException(nameof(libraryInitializer));
            return this;
        }
        
        /// <summary>
        /// Builds an ArchiverManager instance
        /// </summary>
        /// <returns>Configured ArchiverManager</returns>
        /// <exception cref="InvalidOperationException">Thrown when required properties are not set</exception>
        public ArchiverManager Build()
        {
            if (_inputFiles == null)
                throw new InvalidOperationException("Input files must be set");
                
            if (string.IsNullOrEmpty(_outputPath))
                throw new InvalidOperationException("Output path must be set");
                
            return new ArchiverManager(
                _inputFiles,
                _outputPath,
                _profileName,
                _loggerFactory,
                _operationDetector,
                _operationFactory,
                _libraryInitializer
            );
        }
    }
} 