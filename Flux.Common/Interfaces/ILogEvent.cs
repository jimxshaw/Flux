using Flux.Common.Enums;

namespace Flux.Common.Interfaces;

public interface ILogEvent
{
  DateTime Timestamp { get; set; }
  string SourceName { get; set; }
  string RawMessage { get; set; }
  LogSeverity Severity { get; set; }
  IDictionary<string, object> Metadata { get; }
}
