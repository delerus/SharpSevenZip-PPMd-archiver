using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Compression
{
    /// <summary>
    /// Default implementation of IDecompressorFactory that creates SharpSevenZipExtractor instances
    /// </summary>
    public class DefaultDecompressorFactory : IDecompressorFactory
    {
        /// <summary>
        /// Creates a new instance of SharpSevenZipExtractor
        /// </summary>
        /// <param name="archivePath">Path to the archive file</param>
        /// <returns>New SharpSevenZipExtractor instance</returns>
        public SharpSevenZipExtractor CreateDecompressor(string archivePath)
        {
            return new SharpSevenZipExtractor(archivePath);
        }
    }
} 