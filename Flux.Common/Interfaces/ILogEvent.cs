using Flux.Common.Enums;

namespace Flux.Common.Interfaces;

// Represents a single unit of log data that flows through the pipeline.
// This interface abstracts a log message that has been received from a source
// and will be processed and forwarded to a sink.
public interface ILogEvent
{
  DateTime Timestamp { get; set; }
  string SourceName { get; set; }
  string RawMessage { get; set; }
  LogSeverity Severity { get; set; }
  IDictionary<string, object> Metadata { get; }
}
