using Flux.Common.Interfaces;

namespace Flux.Core.Processors;

public class EnrichmentProcessor : ILogProcessor
{
  private readonly string _hostname;
  private readonly string _containerId;
  private readonly int _processId;

  public EnrichmentProcessor()
  {
    _hostname = Environment.MachineName;
    _containerId = getContainerId();
    _processId = Environment.ProcessId;
  }

  public Task<ILogEvent?> ProcessAsync(ILogEvent logEvent, CancellationToken cancellationToken)
  {
    // Enrich the raw message.
    logEvent.RawMessage = $"{logEvent.RawMessage} [host={_hostname} container={_containerId} pid={_processId}]";

    return Task.FromResult<ILogEvent?>(logEvent);
  }

  private string getContainerId()
  {
    // Try to extract container ID from /proc/self/cgroup (Linux/Docker).
    try
    {
      var lines = File.ReadAllLines("/proc/self/cgroup");
      var match = lines.FirstOrDefault(l => l.Contains("docker"));

      if (match != null)
      {
        var parts = match.Split('/');
        return parts.Last().Trim().Substring(0, 12); // Shortened container ID.
      }
    }
    catch
    {
      // Ignore — fallback to hostname.
    }

    return Environment.MachineName;
  }
}
