using Flux.Service;
using NetEscapades.Configuration.Yaml;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
        .AddYamlFile("appsettings.yaml", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

// Register processor worker.
builder.Services.AddHostedService<Processor>();

var host = builder.Build();
host.Run();
