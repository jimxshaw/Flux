using Flux.Common.Options;
using Flux.Common.Interfaces;
using Flux.Core.Sources;

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
        var sourceConfig = _fluxOptions.Value.Sources.FirstOrDefault();
        if (sourceConfig is null)
        {
            _logger.LogError("No source configuration found.");
            return;
        }

        ILogSource source = new UdpLogSource(sourceConfig.Type, sourceConfig.Port);

        await source.StartAsync(
            async (logEvent) =>
            {
                _logger.LogInformation("[SOURCE] Received: {msg}", logEvent.RawMessage);
                // Later: pass to processor chain → sink
                await Task.CompletedTask;
            },
            stoppingToken
        );
    }
}
