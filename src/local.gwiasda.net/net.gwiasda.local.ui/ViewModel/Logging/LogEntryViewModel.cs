using Net.Gwiasda.Logging;
using System.Collections.ObjectModel;

namespace Net.Gwiasda.Local.UI.ViewModel.Logging
{
    public class LogEntryViewModel
    {
        public LogEntryViewModel() { }
        public LogEntryViewModel(LogEntry logEntry)
        {
            if (logEntry == null) throw new ArgumentNullException(nameof(logEntry));
            Id = logEntry.Id;
            LogType = Enum.GetName(logEntry.LogType);
            Timestamp = logEntry.Timestamp.ToString("dd.MM.yyyy hh:mm:ss.fff");
            App = logEntry.AppName;
            Text = logEntry.Message;
            Attributes = logEntry.AdditionalData.Select(nv => new LogEntryAttributeViewModel { Name = nv.Key, Value = nv.Value }).ToList();
        }

        public string? Id { get; set; }
        public string? LogType { get; set; }
        public string? Timestamp { get; set; }
        public string? App { get; set; }
        public string? Text { get; set; }
        public ICollection<LogEntryAttributeViewModel> Attributes { get; private set; } = new Collection<LogEntryAttributeViewModel>();
    }
}