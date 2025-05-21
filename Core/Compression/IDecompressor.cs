using SharpSevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenZipSharpArchiver.Core.Compression
{
    /// <summary>
    /// Interface for file decompressors
    /// </summary>
    public interface IDecompressor
    {
        /// <summary>
        /// Decompresses a file
        /// </summary>
        /// <param name="inputFilePath">Input archive path</param>
        /// <param name="outputFilePath">Output directory path</param>
        void DecompressFile(string inputFilePath, string outputFilePath);
    }
}
