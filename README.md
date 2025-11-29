## Flux Transformation Middleware

### Overview of the Solution's Projects

#### Flux.Forwarder

This project is a simulated UDP log sender. This console app simulates a system that periodically generates log messages and sends them via the UDP protocol to a Flux.Service container.It serves as the "client" in a client-server network model.

#### Flux.Common

This project defines the core abstractions used by all components in the Flux middleware solution.

These abstractions ensure clean separation between:

- Sources: log producers.
- Processors: middleware logic for enrichment, transformations, etc.
- Sinks: log destinations.

Flux.Common is consumed by Flux.Core, the actual implementations, and Flux.Service, the runtime wiring.

#### Flux.Core
This project implements the core functionality of the Flux middleware.

It contains the following implementation modules:
- Models: Concrete data structures.
- Sources: Receives logs.
- Processors: Transforms/enriches logs.
- Sinks: Outputs processed logs.

These implementations are used by Flux.Service to compose the runtime pipeline.

#### Flux.Service
This is the entry point and coordination layer of the Flux middleware system.

It acts as the central server in a client-server model, listening for incoming log events,

processing them through a transformation pipeline, and forwarding them to output sinks.

It uses .NET 8's BackgroundService model to run continuously in a Docker container, configurable via external appsettings.yml.
