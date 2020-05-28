using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModMeta.Core;

namespace ModMetaRelay
{
    public static class StartupExtensions
    {
        internal static IServiceCollection AddPlugins(this IServiceCollection services) {
            var opts = services.BuildServiceProvider().GetService<IOptions<RelayOptions>>();
            var loaders = GetPluginLoaders();
            // Create an instance of plugin types
            foreach (var loader in loaders)
            {
                var types = loader.LoadDefaultAssembly().GetTypes();
                if (types.Any(IsFactory)) {
                    foreach (var pluginType in types.Where(IsFactory))
                    {
                        // This assumes the implementation of IPluginFactory has a parameterless constructor
                        var plugin = Activator.CreateInstance(pluginType) as IModMetaSourceFactory;

                        plugin?.ConfigureServices(services);
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

        private static IEnumerable<PluginLoader> GetPluginLoaders() {
            var loaders = new List<PluginLoader>();
            // create plugin loaders
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (File.Exists(pluginDll))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        pluginDll,
                        sharedTypes: new[] { typeof(IModMetaSourceFactory), typeof(IServiceCollection) });
                    loaders.Add(loader);
                }
            }

            return loaders;
        }

        internal static bool IsFactory(this Type t) {
            return typeof(IModMetaSourceFactory).IsAssignableFrom(t) && !t.IsAbstract;
        }

        internal static bool IsSource(this Type t) {
            return typeof(IModMetaSource).IsAssignableFrom(t) && !t.IsAbstract;
        }
    }
}