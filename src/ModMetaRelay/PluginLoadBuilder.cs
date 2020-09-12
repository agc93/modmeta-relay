using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModMeta.Core;

namespace ModMetaRelay
{
    public interface IPluginBuilder<T> {
        T AddSearchPath(string path);
        T AddSearchPaths(IEnumerable<string> paths);
        T UseConfiguration(IConfiguration configuration);
        T UseConfiguration(IConfigurationSection section);
        T UseLogger(ILogger logger);
        IEnumerable<PluginLoader> Build();
    }
    public class PluginLoadBuilder : IPluginBuilder<PluginLoadBuilder>
    {
        // private ILogger _logger;

        private Action<string> _loggerFunc;

        // private readonly IConfiguration _config;
        private RelayOptions _options {get;set;} = new RelayOptions();

        private List<string> SearchPaths {get;set;} = new List<string>();

        public PluginLoadBuilder()
        {
            SearchPaths.AddRange(GetDefaultPaths());
        }

        private IEnumerable<string> GetDefaultPaths() {
            return new List<string> {
                Path.Combine(AppContext.BaseDirectory, "plugins"),
                Path.Combine(Environment.CurrentDirectory, "plugins")
            };
        }

        public PluginLoadBuilder AddSearchPath(string path) {
            if (!string.IsNullOrWhiteSpace(path)
                    && Directory.Exists(path) 
                    && (Directory.GetDirectories(path).Any() || Directory.GetFiles(path).Any(f => Path.GetExtension(f) == ".dll"))) {
                SearchPaths.Add(Directory.GetFiles(path).Any(f => Path.GetExtension(f) == ".dll") ? Directory.GetParent(path).FullName : path);
            }
            return this;
        }

        public PluginLoadBuilder AddSearchPaths(IEnumerable<string> paths) {
            if (paths.Any()) {
                foreach (var path in paths)
                {
                    this.AddSearchPath(path);
                }
            }
            return this;
        }

        public PluginLoadBuilder UseConfiguration(IConfiguration configuration) {
            _options = configuration
                .GetSection("Relay")?
                .Get<RelayOptions>() ?? new RelayOptions();
            AddConfigSearchPaths();
            return this;
        }

        public PluginLoadBuilder UseConfiguration(IConfigurationSection section) {
            _options = section.Get<RelayOptions>() ?? new RelayOptions();
            AddConfigSearchPaths();
            return this;
        }

        public PluginLoadBuilder UseLogger(ILogger logger) {
            _loggerFunc = msg => logger.LogDebug(msg);
            return this;
        }

        public PluginLoadBuilder UseLogger(Func<ILogger> loggerFunc) {
            _loggerFunc = msg => loggerFunc().LogDebug(msg);
            return this;
        }

        public PluginLoadBuilder UseConsoleLogging() {
            _loggerFunc = msg => Console.WriteLine($"PluginLoader: {msg}");
            return this;
        }

        private void AddConfigSearchPaths() {
            var paths = _options.PluginPaths;
            if (paths.Any()) {
                foreach (var path in paths)
                {
                    AddSearchPath(path);
                }
            }
        }

        private IEnumerable<PluginLoader> BuildLoaders(string pluginsDir) {
            var loaders = new List<PluginLoader>();
            // create plugin loaders
            // var pluginsDir = pluginSearchPath ?? Path.Combine(AppContext.BaseDirectory, "plugins");
            _loggerFunc?.Invoke($"Loading all plugins from {pluginsDir}");
            if (!Directory.Exists(pluginsDir)) return new List<PluginLoader>();
            foreach (var dir in Directory.GetDirectories(pluginsDir).Distinct())
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (File.Exists(pluginDll))
                {
                    _loggerFunc?.Invoke($"Plugin located! Loading {pluginDll}");
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        pluginDll,
                        sharedTypes: new[] { typeof(IModMetaPlugin), typeof(IServiceCollection), typeof(ILogger), typeof(ILogger<>) });
                    loaders.Add(loader);
                }
            }
            return loaders;
        }

        public IEnumerable<PluginLoader> Build() {
            var loaders = SearchPaths.SelectMany(sp => BuildLoaders(sp));
            return loaders;
        }
    }
}