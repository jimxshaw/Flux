using Flux.Common.Options;
using Flux.Common.Interfaces;
using Flux.Core.Sources;
using Flux.Core.Sinks;

using Microsoft.Extensions.Options;
using Flux.Core.Processors;

namespace Flux.Service;

// Core Execution Loop for Flux Service
// This class implements the core log pipeline: Source -> Processor -> Sink.
// It runs inside a background thread using .NET’s BackgroundService.
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
        // Load and initialize all configured sources, processors and sinks.
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
        // Apply transformations (e.g., enrichment) to each event.
        ILogProcessor processor = new EnrichmentProcessor();
        // Output the final logs to one or more sink targets.
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
