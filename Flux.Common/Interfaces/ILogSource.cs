namespace Flux.Common.Interfaces;

// Represents a source of incoming log data.
// Examples: UDP listener, HTTP endpoint, file tailer or API poller.
// Sources are responsible for receiving raw messages and pushing them into the pipeline.
public interface ILogSource
{
  string Name { get; }

  // Starts listening for new logs and emits them using the provided callback.
  // The callback decouples the source from downstream pipeline logic.
  Task StartAsync(Func<ILogEvent, Task> emitCallback, CancellationToken cancellationToken);
}
