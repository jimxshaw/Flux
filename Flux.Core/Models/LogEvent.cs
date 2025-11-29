using Flux.Common;
using Flux.Common.Enums;
using Flux.Common.Interfaces;

namespace Flux.Core.Models;

public class LogEvent : ILogEvent
{
  public DateTime Timestamp { get; set; } = DateTime.UtcNow;
  public string SourceName { get; set; } = string.Empty;
  public string RawMessage { get; set; } = string.Empty;
  public LogSeverity Severity { get; set; } = LogSeverity.Info;

  public IDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
}