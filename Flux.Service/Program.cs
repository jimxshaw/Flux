using Flux.Common.Options;
using Flux.Service;

// This is the main entry point of the Flux middleware host service.
// It uses .NET 8's simplified hosting model via CreateApplicationBuilder.
// The host reads appsettings.yml and registers the main Processor service.
var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
        .AddYamlFile("appsettings.yaml", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

// FluxOptions is configured via YAML to allow flexible source/sink definitions.
// Only one HostedService, Processor.cs, is currently registered but this can be extended.
builder.Services.Configure<FluxOptions>(builder.Configuration.GetSection("Flux"));

// Register the main background processing service.
// This service loops continuously, reading from sources and writing to sinks.
builder.Services.AddHostedService<Processor>();

var host = builder.Build();
host.Run();
