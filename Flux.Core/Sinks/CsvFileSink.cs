using Flux.Common.Interfaces;
using System.Globalization;

namespace Flux.Core.Sinks;

// A file-based sink that appends each log event to a local CSV file.
// Each line represents a single log, formatted as:
//   "Timestamp","SourceName","Severity","RawMessage"
// This simple, human-readable format is great for demos, testing,
// and exporting logs into Excel or other tools for analysis.
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
      File.AppendAllText(_filePath, $"\"Timestamp\",\"SourceName\",\"Severity\",\"RawMessage\"{Environment.NewLine}");
    }
  }

  public string Name => _name;

  // Receive the log event from the processor and puts the data into a .csv file.
  public Task WriteAsync(ILogEvent logEvent, CancellationToken cancellationToken)
  {
    // Remove the new line character.
    var cleanedMessage = logEvent.RawMessage.TrimEnd('\r', '\n');

    // Format the line that will be written into the .csv file.
    var line = $"\"{logEvent.Timestamp}\",\"{logEvent.SourceName}\",\"{logEvent.Severity}\",\"{cleanedMessage}\"{Environment.NewLine}";

    // This lock code block ensures thread safety, where only one thread at a
    // time may enter the code section that writes to the file.
    // It is necessary because multiple log events might arrive concurrently and
    // may experience race conditions, which means two threads overwrite each other's writes.
    lock (_lock)
    {
      // Write the line to the actual .csv file.
      File.AppendAllText(_filePath, line);
    }

    return Task.CompletedTask;
  }
}
