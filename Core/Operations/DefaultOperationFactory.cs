using System;
using SevenZipSharpArchiver.Core.Compression;
using SevenZipSharpArchiver.Core.Logging;
using SevenZipSharpArchiver.Core.Models;

namespace SevenZipSharpArchiver.Core.Operations
{
    /// <summary>
    /// Default implementation of operation factory
    /// </summary>
    public class DefaultOperationFactory : IOperationFactory
    {
        private readonly ILogger _logger;
        private readonly ICompressorFactory _compressorFactory;
        private readonly IDecompressorFactory _decompressorFactory;
        
        /// <summary>
        /// Creates a new instance of DefaultOperationFactory
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="compressorFactory">Factory for creating compressors</param>
        /// <param name="decompressorFactory">Factory for creating decompressors</param>
        public DefaultOperationFactory(
            ILogger logger,
            ICompressorFactory compressorFactory = null,
            IDecompressorFactory decompressorFactory = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _compressorFactory = compressorFactory ?? new DefaultCompressorFactory();
            _decompressorFactory = decompressorFactory ?? new DefaultDecompressorFactory();
        }
        
        /// <summary>
        /// Creates an operation based on the specified type
        /// </summary>
        public IOperation CreateOperation(OperationType type, string profileName = null)
        {
            switch (type)
            {
                case OperationType.Compress:
                    return new CompressOperation(_logger, _compressorFactory, profileName);
                
                case OperationType.Decompress:
                    return new DecompressOperation(_logger, _decompressorFactory);
                
                default:
                    throw new ArgumentException($"Unknown operation type: {type}");
            }
        }
    }
} 