using System.Net.Sockets;
using System.Text;
using Flux.Common.Interfaces;
using Flux.Common.Enums;
using Flux.Core.Models;

namespace Flux.Core.Sources;

public class UdpLogSource : ILogSource
{
  private readonly string _name;
  private readonly int _port;
  private UdpClient? _udpClient;

  public UdpLogSource(string name, int port)
  {
    _name = name;
    _port = port;
  }

  public string Name => _name;

  public async Task StartAsync(Func<ILogEvent, Task> emitCallback, CancellationToken cancellationToken)
  {
    _udpClient = new UdpClient(_port);
    Console.WriteLine($"[UDP Source] Listening on port {_port}...");

    while (!cancellationToken.IsCancellationRequested)
    {
      try
      {
        var result = await _udpClient.ReceiveAsync(cancellationToken);
        var message = Encoding.UTF8.GetString(result.Buffer);

        var logEvent = new LogEvent
        {
          SourceName = _name,
          RawMessage = message,
          Timestamp = DateTime.UtcNow,
          Severity = LogSeverity.Info
        };

        await emitCallback(logEvent);
      }
      catch (OperationCanceledException)
      {
        // Expected during shutdown
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine($"[UDP Source] Error: {ex.Message}");
      }
    }

    _udpClient.Close();
  }
}
