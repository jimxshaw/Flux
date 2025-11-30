namespace Flux.Common.Interfaces;

// Represents a middleware component in the pipeline that processes log events.
// A processor could:
// Normalize formats
// Filter or drop events
// Enrich logs with metadata
// Transform the message
public interface ILogProcessor
{
  // Applies transformation logic to a log event.
  // Return null if the log should be filtered/dropped.
  Task<ILogEvent?> ProcessAsync(ILogEvent logEvent, CancellationToken cancellationToken);
}
