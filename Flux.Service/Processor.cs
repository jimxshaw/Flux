using Flux.Common.Options;
using Flux.Common.Interfaces;
using Flux.Core.Sources;
using Flux.Core.Sinks;

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
        var sinkConfig = _fluxOptions.Value.Sinks.FirstOrDefault();

        if (sourceConfig is null || sinkConfig is null)
        {
            _logger.LogError("Missing source or sink config.");
            return;
        }

        // Set up source and sink.
        ILogSource source = new UdpLogSource(sourceConfig.Type, sourceConfig.Port);
        ILogSink sink = new CsvFileSink(sinkConfig.Path);

        // Start listening.
        await source.StartAsync(
            async (logEvent) =>
            {
                _logger.LogInformation("[SOURCE] Received: {msg}", logEvent.RawMessage);
                await sink.WriteAsync(logEvent, stoppingToken);
            },
            stoppingToken
        );
    }
}
