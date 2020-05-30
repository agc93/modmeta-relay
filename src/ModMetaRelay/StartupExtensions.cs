using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModMeta.Core;

namespace ModMetaRelay
{
    public static class StartupExtensions
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

        private static IEnumerable<PluginLoader> GetPluginLoaders(string pluginSearchPath = null) {
            var loaders = new List<PluginLoader>();
            // create plugin loaders
            var pluginsDir = pluginSearchPath ?? Path.Combine(AppContext.BaseDirectory, "plugins");
            Console.WriteLine($"Loading all plugins from {pluginsDir}");
            if (!Directory.Exists(pluginsDir)) return new List<PluginLoader>();
            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (File.Exists(pluginDll))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        pluginDll,
                        sharedTypes: new[] { typeof(IModMetaPlugin), typeof(IServiceCollection) });
                    loaders.Add(loader);
                }
            }

            return loaders;
        }

        internal static bool IsFactory(this Type t) {
            return typeof(IModMetaPlugin).IsAssignableFrom(t) && !t.IsAbstract;
        }

        internal static bool IsSource(this Type t) {
            return typeof(IModMetaSource).IsAssignableFrom(t) && !t.IsAbstract;
        }
    }
}