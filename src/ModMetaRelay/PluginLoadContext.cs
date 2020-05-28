using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ModMetaRelay
{
    class PluginLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }

    internal class PluginLoader {
        internal IEnumerable<Assembly> GetPluginAssemblies() {
            var pluginPaths = new List<string>() {"./plugins"};
            var plugins = pluginPaths.SelectMany(pluginPath =>
            {
                System.Console.WriteLine($"Loading plugins from {pluginPath}");
                var pluginAssemblies = LoadPlugins(pluginPath);
                return pluginAssemblies;
            }).ToList();
            return plugins;
        }

        private IEnumerable<Assembly> LoadPlugins(string path) {
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pluginLocation = Path.GetFullPath(Path.Combine(root, path.Replace('\\', Path.DirectorySeparatorChar)));
            if (!Directory.Exists(pluginLocation)) return new List<Assembly>();
            System.Console.WriteLine($"Loading commands from: {pluginLocation}");
            var names = Directory.GetFiles(pluginLocation, "*.Plugin.dll", SearchOption.TopDirectoryOnly);
            return names.Select(n => {
                PluginLoadContext loadContext = new PluginLoadContext(n);
                return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(n)));
            });
            // return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }
    }
}