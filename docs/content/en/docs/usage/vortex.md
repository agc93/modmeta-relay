---
title: "Configuring Metaserver Clients"
linkTitle: "Configure Vortex"
weight: 23
description: >
  Configure your clients (such as Vortex) to use your server
---

## Metaserver Clients

Technically, any client that supports the ModMeta API can use the Relay server to fetch metadata.

Currently, Vortex is the only known client to use ModMeta to gather metadata on mods. Other clients will have their own configuration requirements.

## Vortex Configuration

Metaservers can be added to Vortex through two methods: the Settings menu and extensions.

### Settings

To add a new metaserver to Vortex, launch the Settings pane from the sidebar and open the "Download" tab. You'll see a button near the bottom to "Add a Metaserver". Enter the URL of your Relay server and press the tick button on the right hand side to save your new server.

The next time Vortex installs a mod, it will query your new metaserver for any metadata about the mod being installed.

> By default, Vortex will only query for metadata using the file key/hash. You can read more about the different requests in [the developer docs](/docs/developer/modmeta)

### Extensions

Extension authors can programmatically add a companion metaserver using the standard `IExtensionContext` object that your extension uses:

```typescript
function main(context : IExtensionContext) {
    ...
    context.once(() => {
        context.api.addMetaServer('metaserver-id', { url: 'https://your-meta.server.com'});
    }
    ...
}
```

> A word of warning: Vortex versions prior to 1.12.3 had [a nasty bug](https://github.com/Nexus-Mods/Vortex/issues/6315) that meant programmatically removing a metaserver would lead to weird stuff.