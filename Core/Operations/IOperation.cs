using System.Collections.Generic;

namespace SevenZipSharpArchiver.Core.Operations
{
    /// <summary>
    /// Interface for archive operations
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Executes the operation
        /// </summary>
        /// <param name="inputFiles">Input file paths</param>
        /// <param name="outputPath">Output file or directory path</param>
        /// <returns>True if operation was successful</returns>
        bool Execute(IEnumerable<string> inputFiles, string outputPath);
    }
} 