using System;
using System.IO;
using System.Text;

namespace SevenZipSharpArchiver.Core.Logging
{
    /// <summary>
    /// Implementation of ILogger that writes log messages to a file
    /// </summary>
    public class FileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly LogLevel _minLevel;
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the FileLogger
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by this logger</param>
        /// <param name="logFilePath">Path to the log file</param>
        /// <param name="minLevel">The minimum log level to display</param>
        public FileLogger(string categoryName, string logFilePath, LogLevel minLevel = LogLevel.Information)
        {
            _categoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
            _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
            _minLevel = minLevel;
            
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(_logFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Checks if the given LogLevel is enabled
        /// </summary>
        /// <param name="logLevel">Level to be checked</param>
        /// <returns>True if enabled, otherwise false</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLevel;
        }

        /// <summary>
        /// Writes a log entry
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        public void Log(LogLevel logLevel, string message, Exception exception = null)
        {
            if (!IsEnabled(logLevel))
                return;

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var logLevelString = logLevel.ToString().PadRight(11);
            var sb = new StringBuilder();
            
            sb.AppendLine($"[{timestamp}] [{logLevelString}] [{_categoryName}] {message}");
            
            if (exception != null)
            {
                sb.AppendLine($"Exception: {exception.Message}");
                sb.AppendLine($"StackTrace: {exception.StackTrace}");
            }

            try
            {
                lock (_lockObject)
                {
                    File.AppendAllText(_logFilePath, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                // Fallback to console if file writing fails
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
                Console.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// Writes a log entry at Trace level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        public void Trace(string message, Exception exception = null)
        {
            Log(LogLevel.Trace, message, exception);
        }

        /// <summary>
        /// Writes a log entry at Debug level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        public void Debug(string message, Exception exception = null)
        {
            Log(LogLevel.Debug, message, exception);
        }

        /// <summary>
        /// Writes a log entry at Information level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        public void Information(string message, Exception exception = null)
        {
            Log(LogLevel.Information, message, exception);
        }

        /// <summary>
        /// Writes a log entry at Warning level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        public void Warning(string message, Exception exception = null)
        {
            Log(LogLevel.Warning, message, exception);
        }

        /// <summary>
        /// Writes a log entry at Error level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        public void Error(string message, Exception exception = null)
        {
            Log(LogLevel.Error, message, exception);
        }

        /// <summary>
        /// Writes a log entry at Critical level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        public void Critical(string message, Exception exception = null)
        {
            Log(LogLevel.Critical, message, exception);
        }
    }
} 