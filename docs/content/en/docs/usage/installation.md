---
title: "Running the Relay server"
linkTitle: "Run the Server"
weight: 21
description: >
  Set up and run your ModMeta Relay server
---

## Installing ModMeta Relay

There's a couple of ways of running ModMeta Relay at present.

### Docker

We automatically build and publish a Docker image for tagged versions to `quay.io/modmeta-relay/server`. To run the server locally, or on any supported Docker host:

```bash
docker run -d -p <external-port-here>:80 -v /path/to/your/plugins:/app/plugins quay.io/modmeta-relay/server:<your-version-here>
# for example
docker run -d -p 8080:80 -v /tmp/modmeta-plugins:/app/plugins quay.io/modmeta-relay/server:0.1.0
```

The server will be immediately ready to serve mod requests at whatever port you bound to port 80.

> Remember that without any plugins available, the server will still run, but will never return any results!

### Native Packages

We also publish native builds for Windows and Linux through [GitHub Releases](https://github.com/agc93/modmeta-relay/releases). Find the release you want to use, download the correct `.zip` for your platform and extract it somewhere. Create a `Plugins` directory and drop in your plugins, and run `ModMetaRelay`/`ModMetaRelay.exe` to start the server. You can change the listening port in `appSettings.json`, by adding a new key to the configuration:

```json
"urls":"https://0.0.0.0:8888"
```

### CI or Local Builds

If you have the .NET Core runtime already installed, you can use the `dotnet-any` builds available from the [CI builds](https://github.com/agc93/modmeta-relay/actions). Simply download the `modmeta-relay` artifact from a build and run `dotnet ModMetRelay.dll` from the `dotnet-any` directory.

If you have the SDK installed, you can build your own pretty easily by cloning the repo and running the following:

```bash
dotnet tool restore
dotnet cake --bootstrap
dotnet cake --target=Publish
```

This will quickly build all the installation variants locally, into the `dist/` folder.