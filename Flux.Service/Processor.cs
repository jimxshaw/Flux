using Flux.Common.Options;
using Flux.Common.Interfaces;
using Flux.Core.Sources;
using Flux.Core.Sinks;

using Microsoft.Extensions.Options;
using Flux.Core.Processors;

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
        var processorConfig = _fluxOptions.Value.Processors.FirstOrDefault();
        var sinkConfig = _fluxOptions.Value.Sinks.FirstOrDefault();

        if (sourceConfig is null || processorConfig is null || sinkConfig is null)
        {
            _logger.LogError("Missing source, processor or sink config.");
            return;
        }

        // Set up source, processor and sink.
        ILogSource source = new UdpLogSource(sourceConfig.Type, sourceConfig.Port);
        ILogProcessor processor = new EnrichmentProcessor();
        ILogSink sink = new CsvFileSink(sinkConfig.Path);

        // Start listening.
        await source.StartAsync(
            async (logEvent) =>
            {
                var enriched = await processor.ProcessAsync(logEvent, stoppingToken);

                if (enriched != null)
                {
                    await sink.WriteAsync(logEvent, stoppingToken);
                }
            },
            stoppingToken
        );
    }
}
