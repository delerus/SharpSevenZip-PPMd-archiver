using System.Collections.Generic;

namespace SevenZipSharpArchiver.Core.Operations
{
    /// <summary>
    /// Interface for detecting operation type based on input and output paths
    /// </summary>
    public interface IOperationDetector
    {
        /// <summary>
        /// Detects the operation type based on input files and output path
        /// </summary>
        /// <param name="inputFiles">Input file paths</param>
        /// <param name="outputPath">Output file or directory path</param>
        /// <returns>The detected operation type</returns>
        OperationType DetectOperation(IEnumerable<string> inputFiles, string outputPath);
        
        /// <summary>
        /// Checks if a file is an archive
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>True if the file is an archive</returns>
        bool IsArchive(string filePath);
    }
} 