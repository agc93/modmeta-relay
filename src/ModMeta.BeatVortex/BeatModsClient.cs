using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Abstractions;
using Microsoft.Extensions.Caching.InMemory;

namespace ModMeta.BeatVortex
{
    public class BeatModsClient
    {
        
        private readonly HttpClient _client;
        private string LatestGameVersion {get;}
        private readonly JsonSerializerOptions options;
        private readonly IMemoryCache _cache;

        public BeatModsClient(HttpClientFactory clientFactory, IMemoryCache cache) : this(clientFactory)
        {
            _cache = cache;
            if (_cache != null) {
                Task.Run(() => this.CacheLatestGameVersion()).Wait();
            }
        }

        private async Task CacheLatestGameVersion() {
            if (!_cache.TryGetValue("_gameVersion", out var cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = await GetLatestGameVersion();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions() {
                    SlidingExpiration = TimeSpan.FromHours(12)
                };

                // Save data in cache.
                _cache.Set("_gameVersion", cacheEntry, cacheEntryOptions);
            }
        }

        internal static JsonSerializerOptions GetJsonOptions() {
            var opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            opts.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            return opts;
        }

        public BeatModsClient(HttpClientFactory clientFactory)
        {
            options = GetJsonOptions();
            this._client = clientFactory.GetCachedClient(new Uri("https://beatmods.com/api/v1/"));
        }

        private async Task<IEnumerable<BeatModsEntry>> GetMods(string gameVersion = null) {
            var url = string.IsNullOrWhiteSpace(gameVersion)
                ? "mod?search=&status=approved"
                : $"mod?status=approved&gameVersion={gameVersion}";
            var str = await _client.GetStringAsync(url);
            var response = JsonSerializer.Deserialize<List<BeatModsEntry>>(str, options);
            return response;
        }

        private async Task<string> GetLatestGameVersion() {
            if (_cache != null && _cache.TryGetValue<string>("_gameVersion", out var latestVersion)) {
                return latestVersion;
            }
            // var mods = await GetMods();
            var url = "mod?name=BSIPA&status=approved&sort=updatedDate";
            var str = await _client.GetStringAsync(url);
            var mods = JsonSerializer.Deserialize<List<BeatModsEntry>>(str, options);
            var versions = mods.Select(m => m.GameVersion).Where(gv => Version.TryParse(gv, out _)).Select(v => Version.Parse(v)).OrderByDescending(v => v);
            return versions.First().ToString();
        }

        public async Task<IEnumerable<BeatModsEntry>> GetAllMods(string gameVersion = null) {
            return await GetMods(gameVersion);
        }

        public async Task<IEnumerable<BeatModsEntry>> GetModsByName(string name) {
            var mods = await GetMods(await GetLatestGameVersion());
            return mods.Where(m => m.Name.ToLower().Contains(name.ToLower()));
        }

        public async Task<IEnumerable<BeatModsEntry>> GetModsByPattern(string regexPattern) {
            var mods = await GetMods(await GetLatestGameVersion());
            var regex = new System.Text.RegularExpressions.Regex(regexPattern, RegexOptions.IgnoreCase);
            return mods.Where(m => regex.Match(m.Name).Success);
        }

        public async Task<BeatModsEntry> GetLatestModByName(string name, string version = null) {
            var mods = await GetMods(await GetLatestGameVersion());
            return mods.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
        }
    }
}