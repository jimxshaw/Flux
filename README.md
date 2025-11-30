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

### Requirements

Before starting, ensure that **Docker Desktop** is installed and running on your machine.

#### Install Docker Desktop

Choose your operating system:

- [Docker Desktop for Windows](https://docs.docker.com/desktop/install/windows-install/)
- [Docker Desktop for Mac](https://docs.docker.com/desktop/install/mac-install/)
- [Docker Desktop for Linux](https://docs.docker.com/desktop/install/linux-install/)

Or install Docker Engine via command line (for more advanced Linux setups):

```bash
# Ubuntu / Debian
sudo apt update
sudo apt install docker.io docker-compose

# Fedora / RHEL
sudo dnf install docker docker-compose

# Arch Linux
sudo pacman -S docker docker-compose
```

#### Setup Instructions (assuming Docker Desktop is running)

1. Allow execution of the initializer script

  Give the initialize.sh script execution permission:
  
  ```bash
  chmod +x initialize.sh
  ```

2. Run the initialization script

  ```bash
  ./initialize.sh
  ```

  Or if you're on Windows and using PowerShell or the Command Prompt use

  ```sh
  bash ./initialize.sh
  ```

This script will tear down, build and run the necessary Docker containers.

Check the current Terminal Windows and you should see logs being forwarded to the Flux service:

```sh
flux-service-1  | [UDP Source] Listening on port 5140...
flux-forwarder-1  | [Forwarder] Sending logs to flux-service:5140 every 2 seconds...
flux-forwarder-1  | [Forwarder] Sent: Fake log from Forwarder at US Central Time 11/29/2025 15:47:47
flux-forwarder-1  | [Forwarder] Sent: Fake log from Forwarder at US Central Time 11/29/2025 15:47:49
flux-forwarder-1  | [Forwarder] Sent: Fake log from Forwarder at US Central Time 11/29/2025 15:47:51
...
```

Check the **./Flux.Service/logs/output.csv** file and you should see the processed logs written there.

```sh
"Timestamp","SourceName","Severity","RawMessage"
"11/29/2025 21:48:01","Udp","Info","Fake log from Forwarder at US Central Time 11/29/2025 15:48:01 [host=5c2bff82df0f container=5c2bff82df0f pid=1]"
"11/29/2025 21:48:03","Udp","Info","Fake log from Forwarder at US Central Time 11/29/2025 15:48:03 [host=5c2bff82df0f container=5c2bff82df0f pid=1]"
"11/29/2025 21:48:05","Udp","Info","Fake log from Forwarder at US Central Time 11/29/2025 15:48:05 [host=5c2bff82df0f container=5c2bff82df0f pid=1]"
...
```

By seeing both outputs that means the entire log middleware pipeline is working as intended.
