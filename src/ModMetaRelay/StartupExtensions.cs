using System.Collections.Generic;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ModMetaRelay
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Adds all the services required for enabling rate limiting/throttling.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="config">The app config.</param>
        /// <returns>The service container.</returns>
        public static IServiceCollection AddThrottlingServices(this IServiceCollection services, IConfigurationSection config) {
            services.AddMemoryCache();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.Configure<IpRateLimitOptions>(config);

            return services;
        }

        /// <summary>
        /// Adds services for rate limiting/throttling to the app. Includes Swashbuckle whitelisting.
        /// </summary>
        /// <param name="app">The app builder.</param>
        /// <param name="whitelistSwagger">Whether to whitelist Swagger/Swashbuckle endpoints.</param>
        /// <returns>The app builder.</returns>
        public static IApplicationBuilder UseThrottling(this IApplicationBuilder app, bool whitelistSwagger = true) {
            var opts = app.ApplicationServices.GetService<IOptions<IpRateLimitOptions>>();
            opts.Value.EnableEndpointRateLimiting = true;
            if (opts.Value.EndpointWhitelist == null) {
                opts.Value.EndpointWhitelist = new List<string>();
            }
            if (whitelistSwagger) {
                /* var swagger = app.ApplicationServices.GetService<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGeneratorOptions>();
                foreach (var doc in swagger.SwaggerDocs)
                {
                    opts.Value.EndpointWhitelist.Add($"get:/api/{doc.Key}/swagger.json");
                }
                opts.Value.EndpointWhitelist.Add("get:/api/help*"); */
            }
            app.UseIpRateLimiting();
            return app;
        }
    }
}