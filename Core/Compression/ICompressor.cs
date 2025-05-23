using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipSharpArchiver.Core.Compression
{
    /// <summary>
    /// Interface for file compressors
    /// </summary>
    public interface ICompressor
    {
        /// <summary>
        /// Compresses a file
        /// </summary>
        /// <param name="inputFilePath">Input file path</param>
        /// <param name="outputFilePath">Output file path</param>
        void CompressFile(string inputFilePath, string outputFilePath);

        /// <summary>
        /// Compresses multiple files
        /// </summary>
        /// <param name="inputFilePaths">List of input file paths</param>
        /// <param name="outputFilePath">Output archive path</param>
        void CompressFiles(IEnumerable<string> inputFilePaths, string outputFilePath);
    }
}
