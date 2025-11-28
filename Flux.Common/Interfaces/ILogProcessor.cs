namespace Flux.Common.Interfaces;

public interface ILogProcessor
{
  Task<ILogEvent?> ProcessAsync(ILogEvent logEvent, CancellationToken cancellationToken);
}
