using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.Logging
{
    public class LoggingManager : ILoggingManager
    {
        private readonly ILoggingRepository _repository;

        public LoggingManager(ILoggingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task DeleteLogEntriesByAppNameAsync(string appName)
        {
            if(string.IsNullOrWhiteSpace(appName)) throw new ArgumentNullException(nameof(appName));
            await _repository.DeleteLogEntriesByAppNameAsync(appName).ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<string>> GetAppNamesWithLogEntriesAsync()
            => await _repository.GetAppNamesWithLogEntriesAsync().ConfigureAwait(false);

        public async Task<IReadOnlyCollection<LogEntry>> GetLogEntriesByAppNameAsync(string appName)
         => await _repository.GetLogEntriesByAppNameAsync(appName).ConfigureAwait(false);

        public async Task CreateDebugLogEntryAsync(string appName, string message, Dictionary<string, string>? additionalData = null)
            => await InsertLogEntryAsync(appName, message, LogType.Debug, additionalData).ConfigureAwait(false);

        public async Task CreateErrorAsync(string appName, Exception exception, Dictionary<string, string>? additionalData = null)
        {
            if(exception == null) throw new ArgumentNullException(nameof(exception));

            var enrichedData = MergeDictionaries(GetExceptionDetails(exception), additionalData);
            await InsertLogEntryAsync(appName, exception.Message, LogType.Error, enrichedData).ConfigureAwait(false);

            if(exception.InnerException != null)
                await CreateErrorAsync(appName, exception.InnerException, additionalData).ConfigureAwait(false);    
        }

        public async Task CreateInformationLogEntryAsync(string appName, string message, Dictionary<string, string>? additionalData = null)
            => await InsertLogEntryAsync(appName, message, LogType.Information, additionalData);

        public async Task CreateWarningLogEntryAsync(string appName, string message, Dictionary<string, string>? additionalData = null)
            => await InsertLogEntryAsync(appName, message, LogType.Warning, additionalData);
        
        private async Task InsertLogEntryAsync(string appName, string message, LogType logType, Dictionary<string, string>? additionalData = null)
        {
            if (string.IsNullOrWhiteSpace(appName)) throw new ArgumentNullException(nameof(appName));
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException(nameof(message));

            await _repository.InsertLogEntry(new LogEntry
            {
                AppName = appName,
                Message = message,
                LogType = logType,
                AdditionalData = additionalData ?? new Dictionary<string, string>()
            });
        }
        private Dictionary<string, string> GetExceptionDetails(Exception exception)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var exceptionDetails = new Dictionary<string, string>
            {
                { "TargetSite", exception.TargetSite?.Name },
                { "StackTrace", exception.StackTrace }
            };
#pragma warning restore CS8604 // Possible null reference argument.

            if (exception.InnerException != null)
            {
                exceptionDetails.Add("InnerException", exception.InnerException.Message);
            }

            return exceptionDetails;
        }
        private Dictionary<string, string> MergeDictionaries(Dictionary<string, string> dictionary1, Dictionary<string, string>? dictionary2)
        {
            var mergedDictionary = new Dictionary<string, string>(dictionary1);

            if (dictionary2 == null)
                return mergedDictionary;

            foreach (var kvp in dictionary2)
            {
                var key = kvp.Key;
                while (dictionary1.ContainsKey(key))
                {
                    key += "_";
                }
                mergedDictionary.Add(key, kvp.Value);
            }
            return mergedDictionary;
        }
    }
}