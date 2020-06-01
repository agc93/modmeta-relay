---
title: "Relay Plugins"
linkTitle: "Plugins"
weight: 3
description: >
  A basic introduction to Relay Plugins
---

The ModMeta Relay server doesn't store or serve *any* mod metadata. All the metadata that it returns to clients comes from the currently loaded **plugins**.

When the Relay server starts up, it will automatically load all the installed plugins (see below) and use them as metadata sources to respond to incoming requests. For each request, the server will query *all* the available plugins so it's possible for a single request to return multiple different metadata records.

Plugins can selectively support only parts of the ModMeta API if the source they represent doesn't support it, for example.

## Plugin Loading

Plugins are automatically loaded (by default) from a `plugins/` directory in the application root. Plugins must be located in a directory under *plugins* with the same name as the plugin assembly (without the `.dll`). Each plugin can add one or more metadata sources and the server will automatically load all of them and use them to retrieve metadata from requests.

For more detail, check out the [usage docs](/docs/usage/plugins).