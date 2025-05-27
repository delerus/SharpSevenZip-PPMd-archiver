using System;
using System.IO;

namespace SevenZipSharpArchiver.Core.Logging
{
    /// <summary>
    /// Default implementation of ILoggerFactory that creates FileLogger instances
    /// </summary>
    public class DefaultLoggerFactory : ILoggerFactory
    {
        private readonly string _logDirectory;
        private readonly LogLevel _minLevel;
        
        /// <summary>
        /// Initializes a new instance of DefaultLoggerFactory
        /// </summary>
        /// <param name="logDirectory">Directory to store log files, defaults to "logs" in application directory</param>
        /// <param name="minLevel">Minimum log level, defaults to Information</param>
        public DefaultLoggerFactory(string logDirectory = null, LogLevel minLevel = LogLevel.Information)
        {
            _logDirectory = logDirectory ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            _minLevel = minLevel;
            
            // Ensure log directory exists
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }
        
        /// <summary>
        /// Creates a FileLogger with the specified category name
        /// </summary>
        /// <param name="categoryName">Category name for the logger</param>
        /// <returns>FileLogger instance</returns>
        public ILogger CreateLogger(string categoryName)
        {
            string logFileName = $"{categoryName}_{DateTime.Now:yyyyMMdd_HHmmss}.log";
            string logFilePath = Path.Combine(_logDirectory, logFileName);
            return new FileLogger(categoryName, logFilePath, _minLevel);
        }
    }
} 