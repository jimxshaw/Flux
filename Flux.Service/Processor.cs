using Flux.Common.Options;
using Microsoft.Extensions.Options;

namespace Flux.Service;

public class Processor : BackgroundService
{
    private readonly ILogger<Processor> _logger;

    private readonly IOptions<FluxOptions> _fluxOptions;

    public Processor(ILogger<Processor> logger, IOptions<FluxOptions> fluxOptions)
    {
        _logger = logger;
        _fluxOptions = fluxOptions;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var firstSource = _fluxOptions.Value.Sources.FirstOrDefault();

        if (firstSource is not null)
        {
            _logger.LogInformation("Listening on {type} port {port}", firstSource.Type, firstSource.Port);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}
