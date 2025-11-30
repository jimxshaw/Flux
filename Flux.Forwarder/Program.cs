using System.Net.Sockets;
using System.Text;

// Flux.Forwarder: Simulated UDP log sender
// This console app simulates a system that periodically generates log messages
// and sends them via the UDP protocol to a Flux.Service container.
// It serves as the "client" in a client-server network model.

// Configurations
// DNS-resolvable name of the Flux.Service container.
// Docker Compose's internal network allows us to refer to the server container by name.
var targetHost = "flux-service";   // Docker Compose service name = DNS name.
// The UDP port on which Flux.Service is listening.
var targetPort = 5140;
// A new fake log will be sent every 2 seconds.
var interval = TimeSpan.FromSeconds(2);

// Create a UDP client for sending datagrams.
// No binding is required because we're only sending (not receiving).
using var udpClient = new UdpClient();

// Inform the user that the forwarder is running and where logs are being sent.
Console.WriteLine($"[Forwarder] Sending logs to {targetHost}:{targetPort} every {interval.TotalSeconds} seconds...");

// This loop runs indefinitely, sending a new log message at each interval.
while (true)
{
  // Get the current UTC time.
  var utcNow = DateTime.UtcNow;

  // Get the time zone info for US Central Time.
  TimeZoneInfo centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

  // Convert UTC time to US Central Time.
  var usCentralTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, centralTimeZone);

  // Format a simple plaintext log message.
  // In a real-world scenario, this could be a JSON or syslog-formatted log.
  var message = $"Fake log from Forwarder at US Central Time {usCentralTime}";

  // Convert the message to a byte array for UDP transmission.
  var bytes = Encoding.UTF8.GetBytes(message);

  try
  {
    // Send the UDP datagram to the server.
    // This models real-world fire-and-forget log shippers like rsyslog or nxlog.
    await udpClient.SendAsync(bytes, bytes.Length, targetHost, targetPort);
    Console.WriteLine($"[Forwarder] Sent: {message}");
  }
  catch (Exception ex)
  {
    // Catch any transmission errors (e.g., network down, DNS error) and print them.
    Console.WriteLine($"[Forwarder] Error: {ex.Message}");
  }

  // Wait for the specified interval before sending the next message.
  // This simulates a regular log-emitting process, such as a daemon or service.
  await Task.Delay(interval);
}
