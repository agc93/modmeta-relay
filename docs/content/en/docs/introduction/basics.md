---
title: "ModMeta Basics"
linkTitle: "ModMeta Basics"
weight: 2
description: >
  An overview of what the ModMeta API is for
---

The ModMeta API is a simple REST API that at its most basic is an API for providing extended information about mods.

More specifically, ModMeta APIs (aka Metaservers) are used to provide additional information and attributes for specific mods based on details of the mod file itself. They do **not** serve mods, nor make them discoverable, but are specifically for providing metadata for a mod file that a consuming client (Vortex, for example) already knows about.

### `modmeta-db`

While we refer to it as the "ModMeta API", it's not exactly that formalized. The "API" is just the API surfaced by `modmeta-db`, the library that Vortex uses to fetch metadata from Nexus Mods and any other configured metaservers.

The ModMeta DB is both a client and server implementation that serves metadata from a local database or the Nexus Mods API (by default). If you're just looking for a basic working implementation of the ModMeta pattern, then `modmeta-db` is the server for you.

### ModMeta Relay

So what's different about the Relay server? ModMeta Relay intends to be an extensible and configurable server that can provide a ModMeta-compatible API to any other non-Nexus metadata source.

ModMeta Relay doesn't provide any metadata itself! The Relay server does the basics of handling the REST API specifics, query binding and basic validation etc. The actual metadata comes from **plugins** that the server loads, using a ModMeta-like API. Those plugins can then load metadata from a file, database, API or cache and the Relay server will return the results.