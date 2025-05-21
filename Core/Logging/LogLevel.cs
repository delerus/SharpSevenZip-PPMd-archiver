namespace SevenZipSharpArchiver.Core.Logging
{
    /// <summary>
    /// Defines logging severity levels
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Logs that contain the most detailed messages
        /// </summary>
        Trace = 0,

        /// <summary>
        /// Logs that are used for interactive investigation during development
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Logs that track the general flow of the application
        /// </summary>
        Information = 2,

        /// <summary>
        /// Logs that highlight an abnormal or unexpected event in the application flow,
        /// but do not otherwise cause the application execution to stop
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Logs that highlight when the current flow of execution is stopped due to a failure
        /// </summary>
        Error = 4,

        /// <summary>
        /// Logs that describe an unrecoverable application or system crash
        /// </summary>
        Critical = 5,

        /// <summary>
        /// Not used for writing log messages. Specifies that a logging category should not write any messages
        /// </summary>
        None = 6
    }
} 