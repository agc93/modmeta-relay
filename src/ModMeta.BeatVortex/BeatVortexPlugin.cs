using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModMeta.Core;

namespace ModMeta.BeatVortex
{
    public class BeatVortexPlugin : IModMetaPlugin
    {
        public IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<BeatModsClient>();
            services.AddSingleton<IModMetaSource, BeatModsSource>();
            var section = configuration.GetSection("BeatVortex");
            if (section.Exists()) {
                var opts = section.Get<BeatVortexOptions>() ?? new BeatVortexOptions();
                if (opts.EnableBeatSaver) {
                    services.AddSingleton<IModMetaSource, BeatSaverSource>();
                }
            }
            return services;
        }
    }
}
