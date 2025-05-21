using System;

namespace SevenZipSharpArchiver.Core.Logging
{
    /// <summary>
    /// Logging interface used across the application
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Checks if the given LogLevel is enabled
        /// </summary>
        /// <param name="logLevel">Level to be checked</param>
        /// <returns>True if enabled, otherwise false</returns>
        bool IsEnabled(LogLevel logLevel);

        /// <summary>
        /// Writes a log entry
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        void Log(LogLevel logLevel, string message, Exception exception = null);

        /// <summary>
        /// Writes a log entry at Trace level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        void Trace(string message, Exception exception = null);

        /// <summary>
        /// Writes a log entry at Debug level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        void Debug(string message, Exception exception = null);

        /// <summary>
        /// Writes a log entry at Information level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        void Information(string message, Exception exception = null);

        /// <summary>
        /// Writes a log entry at Warning level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        void Warning(string message, Exception exception = null);

        /// <summary>
        /// Writes a log entry at Error level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        void Error(string message, Exception exception = null);

        /// <summary>
        /// Writes a log entry at Critical level
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="exception">Optional exception</param>
        void Critical(string message, Exception exception = null);
    }
} 