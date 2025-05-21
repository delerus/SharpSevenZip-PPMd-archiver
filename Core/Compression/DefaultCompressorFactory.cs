using SharpSevenZip;

namespace SevenZipSharpArchiver.Core.Compression
{
    /// <summary>
    /// Default implementation of ICompressorFactory that creates SharpSevenZipCompressor instances
    /// </summary>
    public class DefaultCompressorFactory : ICompressorFactory
    {
        /// <summary>
        /// Creates a new instance of SharpSevenZipCompressor
        /// </summary>
        /// <returns>New SharpSevenZipCompressor instance</returns>
        public SharpSevenZipCompressor CreateCompressor()
        {
            return new SharpSevenZipCompressor();
        }
    }
} 