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
} 