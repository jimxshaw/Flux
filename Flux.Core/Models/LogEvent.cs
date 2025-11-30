using Flux.Common;
using Flux.Common.Enums;
using Flux.Common.Interfaces;

namespace Flux.Core.Models;

// Concrete implementation of ILogEvent used to represent a log record
// that flows through the pipeline.
// This class stores the timestamp, source info, message content,
// severity, and metadata of a log. It is serializable and extensible,
// making it suitable for enrichment and forwarding.
public class LogEvent : ILogEvent
{
  public DateTime Timestamp { get; set; } = DateTime.UtcNow;
  public string SourceName { get; set; } = string.Empty;
  public string RawMessage { get; set; } = string.Empty;
  public LogSeverity Severity { get; set; } = LogSeverity.Info;

  /// Arbitrary metadata to hold structured context (e.g., hostname, PID).
  /// Used by processors to enrich events with system or runtime info.
  public IDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
}