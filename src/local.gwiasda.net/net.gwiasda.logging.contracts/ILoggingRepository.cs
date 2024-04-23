using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.Logging
{
    public interface ILoggingRepository
    {
        Task DeleteLogEntriesByAppNameAsync(string appName);
        Task<IReadOnlyCollection<string>> GetAppNamesWithLogEntriesAsync();
        Task<IReadOnlyCollection<LogEntry>> GetLogEntriesByAppNameAsync(string appName);
        Task InsertLogEntry(LogEntry logEntry);
    }
}