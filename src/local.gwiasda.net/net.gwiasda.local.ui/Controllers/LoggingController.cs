using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Net.Gwiasda.Local.UI.ViewModel.Logging;
using Net.Gwiasda.Logging;
using System.Collections.ObjectModel;

namespace Net.Gwiasda.Local.UI.Controllers
{
    public class LoggingController : Controller
    {
        private readonly ILoggingManager _loggingManager;

        public LoggingController(ILoggingManager loggingManager)
        {
            _loggingManager = loggingManager ?? throw new ArgumentNullException(nameof(loggingManager));
        }

        public Task<string> Ping() => Task.FromResult($"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")} Pong");

        public async Task<IReadOnlyCollection<string>> GetLogAppNames()
        {
            try
            {
                return await _loggingManager.GetAppNamesWithLogEntriesAsync().ConfigureAwait(true);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync("Logging", exc).ConfigureAwait(true);
                throw;
            }
        }
        public async Task<IReadOnlyCollection<LogEntryViewModel>> GetLoggingEntries(string? appName)
        {
            if (string.IsNullOrWhiteSpace(appName)) throw new ArgumentNullException(nameof(appName));
            try
            {
                if (_loggingManager == null) throw new InvalidOperationException();
                return new ReadOnlyCollection<LogEntryViewModel>((await _loggingManager.GetLogEntriesByAppNameAsync(appName).ConfigureAwait(true))
                    .Select(le => new LogEntryViewModel(le)).ToList());
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(appName, exc).ConfigureAwait(true);
                throw;
            }
        }
        public async Task DeleteLogs(string? appName)
        {
            if(string.IsNullOrWhiteSpace(appName)) throw new ArgumentNullException(nameof(appName));
            try
            {
                await _loggingManager.DeleteLogEntriesByAppNameAsync(appName);
            }
            catch (Exception exc)
            {
                await _loggingManager.CreateErrorAsync(appName, exc).ConfigureAwait(true);
                throw;
            }
        }
    }
}