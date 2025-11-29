namespace Flux.Common.Interfaces;

public interface ILogSink
{
  string Name { get; }

  Task WriteAsync(ILogEvent logEvent, CancellationToken cancellationToken);
}
