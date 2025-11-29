namespace Flux.Common.Options;

// Represents configuration settings loaded from appsettings.yaml.
// Includes lists of log sources and sinks to be initialized at runtime.
public class FluxOptions
{
  public List<SourceOptions> Sources { get; set; } = new();
  public List<ProcessorOptions> Processors { get; set; } = new();
  public List<SinkOptions> Sinks { get; set; } = new();
}

public class SourceOptions
{
  public string Type { get; set; } = string.Empty;
  public int Port { get; set; }
}

public class ProcessorOptions
{
  public string Name { get; set; } = string.Empty;
}

public class SinkOptions
{
  public string Type { get; set; } = string.Empty;
  public string Path { get; set; } = string.Empty;
}