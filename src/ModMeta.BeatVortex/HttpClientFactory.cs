using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.Extensions.Caching.Abstractions;
using Microsoft.Extensions.Caching.InMemory;

namespace ModMeta.BeatVortex
{
    public class HttpClientFactory
    {
        public HttpMessageHandler GetCachedHandler() {
            var handler = new HttpClientHandler {
                AllowAutoRedirect = true
            };
            var cacheExpirationPerHttpResponseCode = CacheExpirationProvider.CreateSimple(TimeSpan.FromHours(12), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(60));
            var cacheHandler = new InMemoryCacheHandler(handler, cacheExpirationPerHttpResponseCode);
            return cacheHandler;
        }

        public HttpClient GetCachedClient(Uri baseAddress = null, string userAgent = null) {
            var handler = GetCachedHandler();
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent ?? GetUserAgent());
            if (baseAddress != null && !string.IsNullOrWhiteSpace(baseAddress.ToString())) {
                client.BaseAddress = baseAddress;
            }
            return client;
        }

        private string GetUserAgent() {
            return $"BeatVortex/{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
        }
    }
}