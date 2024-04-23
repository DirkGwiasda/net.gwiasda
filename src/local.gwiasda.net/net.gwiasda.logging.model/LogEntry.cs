using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net.Gwiasda.Logging
{
    public class LogEntry : IComparable<LogEntry>, IEquatable<LogEntry>
    {
        public static LogEntry Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            return JsonSerializer.Deserialize<LogEntry>(json) ?? throw new InvalidOperationException("Cannot deserialize LogEntry.");
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public LogType LogType { get; set; } = LogType.None;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string AppName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string> AdditionalData { get; set; } = new Dictionary<string, string>();

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public int CompareTo(LogEntry? other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            return Timestamp.CompareTo(other.Timestamp);
        }

        public bool Equals(LogEntry? other)
        {
            if (other == null) return false;
            if (Id != other.Id) return false;
            if (LogType != other.LogType) return false;
            if (Timestamp != other.Timestamp) return false;
            if (AppName != other.AppName) return false;
            if (Message != other.Message) return false;
            if (AdditionalData?.Count != other.AdditionalData?.Count) return false;
            if (AdditionalData != null && other.AdditionalData != null)
            {
                foreach (var attribute in AdditionalData)
                {
                    if (!other.AdditionalData.ContainsKey(attribute.Key)) return false;
                    if (attribute.Value != other.AdditionalData[attribute.Key]) return false;
                }
            }

            return true;
        }
    }
}
