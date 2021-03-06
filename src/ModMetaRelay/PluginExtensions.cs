﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModMeta.Core;

namespace ModMetaRelay
{
    public static class PluginExtensions
    {
        internal static IServiceCollection AddPlugins(this IServiceCollection services, IConfiguration configuration) {
            var opts = configuration
                .GetSection("Relay")?
                .Get<RelayOptions>() ?? new RelayOptions();
            // var loaders = Enumerable.Empty<PluginLoader>();
            // var opts = services.BuildServiceProvider().GetService<IOptions<RelayOptions>>();
            var loaders = new PluginLoadBuilder()
                .UseConfiguration(configuration.GetSection("Relay"))
                .UseConsoleLogging()
                .Build();
            // Create an instance of plugin types
            foreach (var loader in loaders)
            {
                var types = loader.LoadDefaultAssembly().GetTypes();
                if (types.Any(IsFactory)) {
                    foreach (var pluginType in types.Where(IsFactory))
                    {
                        // This assumes the implementation of IPluginFactory has a parameterless constructor
                        var plugin = Activator.CreateInstance(pluginType) as IModMetaPlugin;

                        plugin?.ConfigureServices(services, configuration);
                    }
                } else {
                    foreach (var sourceType in types.Where(IsSource))
                    {
                        // This assumes the implementation of IPluginFactory has a parameterless constructor
                        var plugin = Activator.CreateInstance(sourceType) as IModMetaSource;

                        services.AddSingleton<IModMetaSource>(plugin);
                    }
                }
            }
            return services;
        }

        internal static bool IsFactory(this Type t) {
            return typeof(IModMetaPlugin).IsAssignableFrom(t) && !t.IsAbstract;
        }

        internal static bool IsSource(this Type t) {
            return typeof(IModMetaSource).IsAssignableFrom(t) && !t.IsAbstract;
        }
    }
}