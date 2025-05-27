using System;
using System.Collections.Generic;

namespace SevenZipSharpArchiver.Api
{
    /// <summary>
    /// Represents the result of an archive operation
    /// </summary>
    public class ArchiveResult
    {
        /// <summary>
        /// Gets or sets whether the operation was successful
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// Gets or sets a message describing the result
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Gets or sets the exception that occurred during the operation, if any
        /// </summary>
        public Exception Exception { get; set; }
    }
    
    /// <summary>
    /// Represents the result of an archive operation with additional file information
    /// </summary>
    public class ArchiveFileResult : ArchiveResult
    {
        /// <summary>
        /// Gets or sets the list of files processed during the operation
        /// </summary>
        public List<string> ProcessedFiles { get; set; } = new List<string>();
        
        /// <summary>
        /// Gets or sets the total size of the processed files in bytes
        /// </summary>
        public long TotalSizeBytes { get; set; }
        
        /// <summary>
        /// Gets or sets the compressed size in bytes (for compression operations)
        /// </summary>
        public long CompressedSizeBytes { get; set; }
        
        /// <summary>
        /// Gets the compression ratio (original size / compressed size)
        /// </summary>
        public double CompressionRatio => TotalSizeBytes > 0 && CompressedSizeBytes > 0
            ? (double)TotalSizeBytes / CompressedSizeBytes
            : 0;
    }
} 