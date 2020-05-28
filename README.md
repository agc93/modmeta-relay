# ModMeta Relay

> This is *not* a Nexus-supported project.

A simple .NET Core-based relay/proxy server to use as a Metadata Server in Vortex.

On it's own, the server won't return any results as it relies on plugins (metadata sources) to return results from _wherever_. Plugins are loaded dynamically on startup from the `Plugins/` folder.

Metadata sources are implementations of `IModMetaSource` to reflect the API of "normal" `modmeta-db`. Plugins can alternatively implement `IModMetaSourceFactory` to register their own mod sources or other required services with the DI container.

