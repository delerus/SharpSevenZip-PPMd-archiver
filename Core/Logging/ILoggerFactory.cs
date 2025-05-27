using System;

namespace SevenZipSharpArchiver.Core.Logging
{
    /// <summary>
    /// Factory for creating logger instances
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// Creates a logger with the specified category name
        /// </summary>
        /// <param name="categoryName">Category name for the logger</param>
        /// <returns>Logger instance</returns>
        ILogger CreateLogger(string categoryName);
    }
} 