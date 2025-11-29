namespace Flux.Common.Interfaces;

public interface ILogSource
{
  string Name { get; }

  Task StartAsync(Func<ILogEvent, Task> emitCallback, CancellationToken cancellationToken);
}
