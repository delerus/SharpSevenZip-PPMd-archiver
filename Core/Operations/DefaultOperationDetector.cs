using System.Collections.Generic;
using System.IO;
using System.Linq;
using SevenZipSharpArchiver.Core.Logging;

namespace SevenZipSharpArchiver.Core.Operations
{
    /// <summary>
    /// Default implementation of operation detector
    /// </summary>
    public class DefaultOperationDetector : IOperationDetector
    {
        private readonly string[] _archiveExtensions = { ".7z", ".zip", ".rar", ".tar", ".gz", ".bz2", ".xz", ".cab", ".iso" };
        private readonly ILogger _logger;
        
        /// <summary>
        /// Creates a new instance of DefaultOperationDetector
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public DefaultOperationDetector(ILogger logger = null)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Detects the operation type based on input files and output path
        /// </summary>
        public OperationType DetectOperation(IEnumerable<string> inputFiles, string outputPath)
        {
            if (inputFiles == null || !inputFiles.Any())
            {
                _logger?.Error("No input files specified");
                throw new System.ArgumentException("No input files specified");
            }
            
            // If we have a single input file and it's an archive, we're decompressing
            if (inputFiles.Count() == 1 && IsArchive(inputFiles.First()))
            {
                _logger?.Information($"Detected decompression operation for {inputFiles.First()}");
                return OperationType.Decompress;
            }
            
            // Otherwise, we're compressing
            _logger?.Information($"Detected compression operation for {inputFiles.Count()} file(s)");
            return OperationType.Compress;
        }
        
        /// <summary>
        /// Checks if a file is an archive
        /// </summary>
        public bool IsArchive(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            
            string extension = Path.GetExtension(filePath).ToLower();
            return _archiveExtensions.Contains(extension);
        }
    }
} 