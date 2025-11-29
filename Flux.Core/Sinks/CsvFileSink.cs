using Flux.Common.Interfaces;
using System.Globalization;

namespace Flux.Core.Sinks;

public class CsvFileSink : ILogSink
{
  private readonly string _name = "CsvFileSink";
  private readonly string _filePath;
  private readonly object _lock = new(); // Thread-safe writes.

  public CsvFileSink(string filePath)
  {
    _filePath = filePath;

    // Ensure log folder exists.
    var directory = Path.GetDirectoryName(_filePath);
    if (!string.IsNullOrEmpty(directory))
    {
      Directory.CreateDirectory(directory);
    }

    // Write header if file doesn't exist.
    if (!File.Exists(_filePath))
    {
      File.AppendAllText(_filePath, "Timestamp,SourceName,Severity,RawMessage\n");
    }
  }

  public string Name => _name;

  public Task WriteAsync(ILogEvent logEvent, CancellationToken cancellationToken)
  {
    var line = string.Format(
        CultureInfo.InvariantCulture,
        "\"{0:O}\",\"{1}\",\"{2}\",\"{3}\"{4}",
        logEvent.Timestamp,
        Escape(logEvent.SourceName),
        logEvent.Severity,
        Escape(logEvent.RawMessage),
        Environment.NewLine
    );

    lock (_lock)
    {
      File.AppendAllText(_filePath, line);
    }

    return Task.CompletedTask;
  }

  private static string Escape(string input)
  {
    return input.Replace("\"", "\"\""); // Escape quotes for CSV.
  }
}
