namespace Flux.Common.Interfaces;

// Represents a destination that receives and stores log events after processing.
// Examples include writing to a file, pushing to a SIEM (like Sentinel), or forwarding to Kafka.
public interface ILogSink
{
  string Name { get; }

  // Accepts a log event and writes or forwards it to a destination.
  Task WriteAsync(ILogEvent logEvent, CancellationToken cancellationToken);
}
