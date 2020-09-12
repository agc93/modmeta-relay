---
title: "Building Plugins"
linkTitle: "Plugins"
weight: 92
description: >
  An introduction to building Relay plugins.
---

## Introduction

At their most basic, a Relay plugin is simply an assembly with an implementation of `IModMetaSource` (from `ModMeta.Core`), and optionally an implementation of `IModMetaPlugin`.

The `IModMetaSource` interface is how the Relay server passes mod metadata queries to your plugin and handles the responses to pass on to the original client.

## Implementing a metadata source

In short, add a reference to [`ModMeta.Core`](https://www.nuget.org/packages/ModMeta/) and implement the `IModMetaSource` interface. The various `GetBy*` methods are the main entry points the server will call when it gets a request for a mod.

### Request Type Support

Since not every [request type](/docs/developer/modmeta/#request-types) might make sense for the metadata source you're building, plugins can choose to only work with certain request types.

If your plugin doesn't support a request type *at all*, not returning it from the `SupportedTypes` property will cause the server to never even call your plugin for that type. If you only want to skip *certain* requests, throwing a `NotImplementedException` will cause the server to ignore your plugin's results for that request.

### Async and timeouts

All configured plugins are called by the server at the same time for each request. If you're familiar with the TPL, we're currently using `Task.WhenAll()` so your plugin will *not* be running in a different thread. To keep UX non-awful, there's a hard limit configured of 5 seconds after which any plugins that haven't returned their results are ignored. Given metaservers calls often block installation (as they do in Vortex), making your plugin return data as fast as possible will lead to a much better experience.

## Complex Plugins and `IModMetaPlugin`

If your mod has more advanced requirements or you want to take advantage of runtime DI, you can register your plugin using an implementation of `IModMetaPlugin`. Much like a metadata source (`IModMetaSource`), the server will dynamically load your plugin and locate your implementation of `IModMetaPlugin`.

> If you use `IModMetaPlugin`, the server will *not* automatically register your `IModMetaSource` implementation, you need to do that yourself in the `ConfigureServices` method.

Using the `IModMetaPlugin.ConfigureServices` method, you can register any dependencies you need or read configuration from the host server. For example:

```csharp
public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddSingleton<BeatModsClient>();
    services.AddSingleton<IModMetaSource, BeatModsSource>();
    return services;
}
```

It's worth noting that (with some exceptions) you won't be able to access all the services from the host server's DI container. The exceptions to this are `IConfiguration` and `ILogger`. This means you can add the `Microsoft.Extensions.Logging` package to your plugin then just inject `ILogger<>` into any of your plugin types to write out logs.