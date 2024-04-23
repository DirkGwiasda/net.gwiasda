﻿using Net.Gwiasda.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net.Gwiasda.Local.Repository
{
    public class FileSystemLoggingRepository : ILoggingRepository
    {
        internal const string RootDataDirectory = "gwiasda_local_data";
        internal const string LoggingDirectory = "logging";
        internal const string FileExtension = ".log";

        private readonly object _sync = new object();

        public Task DeleteLogEntriesByAppNameAsync(string appName)
        {
            lock (_sync)
            {
                if (File.Exists(GetFQFileName(appName)))
                    File.Delete(GetFQFileName(appName));
            }
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<string>> GetAppNamesWithLogEntriesAsync()
        {
            var result = new List<string>();
            lock (_sync) 
            {
                var files = Directory.GetFiles(GetBaseDirectory());
                foreach(var file in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    result.Add(fileName);
                }
            }
            return Task.FromResult<IReadOnlyCollection<string>>(result);
        }

        public Task<IReadOnlyCollection<LogEntry>> GetLogEntriesByAppNameAsync(string appName)
        {
            lock(_sync)
            {
                return Task.FromResult<IReadOnlyCollection<LogEntry>>(GetLogEntriesByAppName(appName));
            }
            
        }

        public Task InsertLogEntry(LogEntry logEntry)
        {
            lock(_sync)
            {
                var logEntries = GetLogEntriesByAppName(logEntry.AppName);
                logEntries.Add(logEntry);

                if(!Directory.Exists(GetBaseDirectory()))
                    Directory.CreateDirectory(GetBaseDirectory());

                File.WriteAllText(GetFQFileName(logEntry.AppName), JsonSerializer.Serialize(logEntries));
            }
            return Task.CompletedTask;
        }

        private List<LogEntry> GetLogEntriesByAppName(string appName)
        {
            string? json = null;
               if (File.Exists(GetFQFileName(appName)))
                    json = File.ReadAllText(GetFQFileName(appName));
            if(json == null)
                return new List<LogEntry>();

            return JsonSerializer.Deserialize<List<LogEntry>>(json) ?? new List<LogEntry>();
        }

        private string GetBaseDirectory() => Path.Combine(RootDataDirectory, LoggingDirectory);
        private string GetFQFileName(string fileName) => Path.Combine(GetBaseDirectory(), $"{fileName}{FileExtension}");
    }
}