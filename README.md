# ModMeta Relay

ModMeta Relay is an **unofficial** implementation of the ModMeta API for interfacing Vortex (or any other ModMeta client) with any non-Nexus mod source. To clarify, this project is not affiliated in any way with Nexus Mods or anyone else, and is an open-source community resource.

So what's different about the Relay server? ModMeta Relay intends to be an extensible and configurable server that can provide a ModMeta-compatible API to any other non-Nexus metadata source.

ModMeta Relay doesn't provide any metadata itself! The actual metadata comes from **plugins** that can then load metadata from a file, database, API or cache and the Relay server will return the results.

Full documentation for getting started with ModMeta Relay as well as how to build plugins are available [agc93.github.io/modmeta-relay/](https://agc93.github.io/modmeta-relay/).