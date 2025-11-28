namespace Flux.Service;

public class Processor : BackgroundService
{
    private readonly ILogger<Processor> _logger;
    private readonly IConfiguration _config;

    public Processor(ILogger<Processor> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                var port = _config.GetValue<int>("Flux:Sources:0:Port");
                _logger.LogInformation("Listening on UDP port {port}", port);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
