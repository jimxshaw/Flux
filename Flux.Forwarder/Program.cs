using System.Net.Sockets;
using System.Text;

var targetHost = "flux-service";   // Docker Compose service name = DNS name.
var targetPort = 5140;
var interval = TimeSpan.FromSeconds(2);

using var udpClient = new UdpClient();

Console.WriteLine($"[Forwarder] Sending logs to {targetHost}:{targetPort} every {interval.TotalSeconds} seconds...");

while (true)
{
  // Get the current UTC time.
  var utcNow = DateTime.UtcNow;

  // Get the time zone info for US Central Time.
  TimeZoneInfo centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

  // Convert UTC time to US Central Time.
  var usCentralTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, centralTimeZone);

  var message = $"Fake log from Forwarder at US Central Time {usCentralTime}";
  var bytes = Encoding.UTF8.GetBytes(message);

  try
  {
    await udpClient.SendAsync(bytes, bytes.Length, targetHost, targetPort);
    Console.WriteLine($"[Forwarder] Sent: {message}");
  }
  catch (Exception ex)
  {
    Console.WriteLine($"[Forwarder] Error: {ex.Message}");
  }

  await Task.Delay(interval);
}
