---
title: "Adding Plugins"
linkTitle: "Adding Plugins"
weight: 22
description: >
  Adding plugins to your ModMeta Relay server
---

## ModMeta Plugins

If you run the ModMeta Relay server without any plugins configured, you will never get any results! Plugins are responsible for getting mod metadata from whatever source(s) they support.

## Installing Plugins

There's no special process for "installing" plugins or registering them to the server. Simply place them in a `plugins/` directory beside the server binary and the server will automatically load them. Plugins must be in a *directory* named the same as the plugin. For example:

```bash
win-x64
    ├── appsettings.Development.json
    ├── appsettings.json
    ├── ModMetaRelay.exe
    ├── ModMetaRelay.pdb
    └── plugins
        └── ModMeta.BeatVortex
            ├── ModMeta.BeatVortex.dll
            ├── ModMeta.BeatVortex.deps.json
            ...
```

> Plugins are loaded only when the server starts! If you've added new plugins, make sure you restart the server to load them.

### Plugin Loading

If your plugin doesn't seem to be loading with the server, make sure you check the output/logs. During startup the server will log the locations of plugins it has discovered as well as the paths it is searching.

It's also possible to load multiple copies of the same plugin! If it appears in multiple search locations, the Relay server will load it from each location as all plugins are isolated from each other.

## Plugin Configuration

The Relay server (optionally) picks up it's configuration from a `modmeta.json` file in the app directory. If you need to put your plugins in a different location, you can add extra paths for plugins using the `PluginPaths` option:

```json
{
  ...
  "Relay": {
    "PluginPaths": [
      "/additional/path/to/plugins"
    ]
  }
  ...
}
```


### Docker

The Docker image does not include *any* plugins by default. You need to include your plugins either using a custom `Dockerfile` or by simply mounting a plugins folder to the `/app/plugins/` directory in your container:

```bash
# for example
docker run -d -p 8080:80 -v /tmp/modmeta-plugins:/app/plugins quay.io/modmeta-relay/server:0.1.0
```