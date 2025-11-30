using System.Net.Sockets;
using System.Text;
using Flux.Common.Interfaces;
using Flux.Common.Enums;
using Flux.Core.Models;

namespace Flux.Core.Sources;

// A simple UDP log source that listens on a given port for incoming datagrams.
// It parses each UDP packet as a plaintext log message and emits it into the pipeline.
// This source simulates real-world systems like rsyslog, where logs are shipped
// via UDP without guaranteed delivery.
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
    // Initialize a new UDP client.
    _udpClient = new UdpClient(_port);
    Console.WriteLine($"[UDP Source] Listening on port {_port}...");

    // Keep listening for incoming UDP datagrams at the set port
    // until a cancel token is provided.
    while (!cancellationToken.IsCancellationRequested)
    {
      try
      {
        // Get the whole UDP datagram.
        var result = await _udpClient.ReceiveAsync(cancellationToken);

        // Get the actual message.
        var message = Encoding.UTF8.GetString(result.Buffer);

        // Map the message to our internal log event class.
        var logEvent = new LogEvent
        {
          SourceName = _name,
          RawMessage = message,
          Timestamp = DateTime.UtcNow,
          Severity = LogSeverity.Info
        };

        // Forward the log event object to the next receiver.
        await emitCallback(logEvent);
      }
      catch (OperationCanceledException)
      {
        // Expected during shutdown.
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine($"[UDP Source] Error: {ex.Message}");
      }
    }

    // Close the UDP client when finished.
    _udpClient.Close();
  }
}
