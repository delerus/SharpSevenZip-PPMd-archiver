using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Compression
{
    /// <summary>
    /// Factory interface for creating SharpSevenZipExtractor instances
    /// </summary>
    public interface IDecompressorFactory
    {
        /// <summary>
        /// Creates a new instance of SharpSevenZipExtractor
        /// </summary>
        /// <param name="archivePath">Path to the archive file</param>
        /// <returns>New SharpSevenZipExtractor instance</returns>
        SharpSevenZipExtractor CreateDecompressor(string archivePath);
    }
} 