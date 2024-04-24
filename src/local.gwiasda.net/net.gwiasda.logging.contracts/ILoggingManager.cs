namespace Net.Gwiasda.Logging
{
    /// <summary>
    /// Represents a logging manager.
    /// </summary>
    public interface ILoggingManager
    {
        /// <summary>
        /// Inserts an error log entry asynchronously.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="additionalData">Additional data to include in the log entry.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateErrorAsync(string appName, Exception exception, Dictionary<string, string>? additionalData = null);

        /// <summary>
        /// Inserts a debug log entry asynchronously.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <param name="message">The log message.</param>
        /// <param name="additionalData">Additional data to include in the log entry.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateDebugLogEntryAsync(string appName, string message, Dictionary<string, string>? additionalData = null);

        /// <summary>
        /// Inserts an information log entry asynchronously.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <param name="message">The log message.</param>
        /// <param name="additionalData">Additional data to include in the log entry.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateInformationLogEntryAsync(string appName, string message, Dictionary<string, string>? additionalData = null);

        /// <summary>
        /// Inserts a warning log entry asynchronously.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <param name="message">The log message.</param>
        /// <param name="additionalData">Additional data to include in the log entry.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateWarningLogEntryAsync(string appName, string message, Dictionary<string, string>? additionalData = null);

        /// <summary>
        /// Gets the application names with log entries asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<IReadOnlyCollection<string>> GetAppNamesWithLogEntriesAsync();

        /// <summary>
        /// Gets the log entries by application name asynchronously.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<IReadOnlyCollection<LogEntry>> GetLogEntriesByAppNameAsync(string appName);

        /// <summary>
        /// Deletes the log entries by application name asynchronously.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteLogEntriesByAppNameAsync(string appName);
    }
}