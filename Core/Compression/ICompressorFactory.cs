using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Compression
{
    /// <summary>
    /// Factory interface for creating SharpSevenZipCompressor instances
    /// </summary>
    public interface ICompressorFactory
    {
        /// <summary>
        /// Creates a new instance of SharpSevenZipCompressor
        /// </summary>
        /// <returns>New SharpSevenZipCompressor instance</returns>
        SharpSevenZipCompressor CreateCompressor();
    }
} 