---
title: "Frequently Asked Questions"
linkTitle: "FAQ"
weight: 20
---

Below is a collection of frequent questions and the best answers I can give.

### Why should I use this?

If your integration/extension/game/mod is using Vortex and mods hosted on Nexus Mods, you probably don't need this! ModMeta Relay is specifically intended at making it easier to support non-Nexus mod sources with metadata for Vortex (or any other ModMeta client).

### Why not just use modmeta-db?

Use what you prefer! I was looking for something a little less structured/opinionated than the basic `modmeta-db`, so designed this as an alternative.

Ideally, from a client's point of view, it shouldn't even be obvious whether your metaserver is using `modmeta-db` or the ModMeta Relay.

### How do I get ModMeta relay to use metadata from <x>?

Unless you know there's already a plugin for the source in question, you will probably need to build one. This is a reasonably simple process and the server should pick up any available plugins at startup.

There's a ton of docs on building for/with Relay in [the developer docs](/docs/developer).

### Why are there so many references to Beat Saber/BeatVortex in here?

Just like how `modmeta-db` was born out of the Vortex project but is now a separate component, ModMeta Relay was originally a component of BeatVortex until I realised it could be split out as a game- and source-agnostic metadata server. In particular, the BeatVortex plugin is still currently part of this repository, but will be moved at a later date.