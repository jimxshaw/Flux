namespace Flux.Common.Options;

public class FluxOptions
{
  public List<SourceOptions> Sources { get; set; } = new();
  public List<SinkOptions> Sinks { get; set; } = new();
}

public class SourceOptions
{
  public string Type { get; set; } = string.Empty;
  public int Port { get; set; }
}

public class SinkOptions
{
  public string Type { get; set; } = string.Empty;
  public string Path { get; set; } = string.Empty;
}