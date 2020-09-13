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
using Microsoft.Extensions.Logging;

namespace ModMeta.BeatVortex
{
    internal static class CacheKeys {
        public static string AllMods => "_allMods";
        public static string GameVersion => "_gameVersion";
    }
    public class BeatModsClient
    {
        
        private readonly HttpClient _client;

        private string LatestGameVersion {get;}
        private readonly JsonSerializerOptions options;
        private readonly ILogger<BeatModsClient> _logger;
        private readonly IVersionProvider _versionProvider;
        private readonly IMemoryCache _cache;

        public BeatModsClient(HttpClientFactory clientFactory, IMemoryCache cache, JsonSerializerOptions options, IVersionProvider versionProvider, Microsoft.Extensions.Logging.ILogger<BeatModsClient> logger)
        {
            this._client = clientFactory.GetClient(new Uri("https://beatmods.com/api/v1/"));
            _cache = cache;
            _logger = logger;
            _versionProvider = versionProvider;
            this.options = options;
            if (_cache != null) {
                Task.Run(() => this.CacheLatestGameVersion()).Wait();
            }
        }

        private async Task CacheLatestGameVersion() {
            if (!_cache.TryGetValue(CacheKeys.GameVersion, out var cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = await _versionProvider.GetLatestVersion();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions() {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
                    // SlidingExpiration = TimeSpan.FromHours(12)
                };

                _logger.LogInformation($"Caching latest game version at '{cacheEntry}' for 6 hours");

                // Save data in cache.
                _cache.Set(CacheKeys.GameVersion, cacheEntry, cacheEntryOptions);
            }
        }

        private void CacheAllMods(List<BeatModsEntry> mods) {
            _logger.LogWarning("Rebuilding cache of all mods!");
            if (_cache != null) {

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions() {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
                    // SlidingExpiration = TimeSpan.FromHours(12)
                };
                _logger.LogDebug($"Caching {mods.Count} mods in memory for 6 hours");

                // Save data in cache.
                _cache.Set(CacheKeys.AllMods, mods, cacheEntryOptions);
            }
        }

        private async Task<IEnumerable<BeatModsEntry>> GetMods(string gameVersion = null) {
            if (string.IsNullOrWhiteSpace(gameVersion)) {
                gameVersion = _cache.TryGetValue<string>("_gameVersion", out var version)
                    ? version
                    : await _versionProvider.GetLatestVersion();
            }
            if (_cache != null && _cache.TryGetValue<List<BeatModsEntry>>(CacheKeys.AllMods, out var allMods)) {
                _logger.LogDebug($"Returning {allMods.Count} mods response from cache!");
                return allMods;
            }
            var url = string.IsNullOrWhiteSpace(gameVersion)
                ? "mod?search=&status=approved"
                : $"mod?status=approved&gameVersion={gameVersion}";
            var str = await _client.GetStringAsync(url);
            var response = JsonSerializer.Deserialize<List<BeatModsEntry>>(str, options);
            CacheAllMods(response);
            return response;
        }

        public async Task<IEnumerable<BeatModsEntry>> GetAllMods(string gameVersion = null) {
            return await GetMods(gameVersion);
        }

        public async Task<IEnumerable<BeatModsEntry>> GetModsByName(string name) {
            var mods = await GetMods();
            return mods.Where(m => m.Name.ToLower().Contains(name.ToLower()));
        }

        public async Task<IEnumerable<BeatModsEntry>> GetModsByPattern(string regexPattern) {
            var mods = await GetMods();
            var regex = new System.Text.RegularExpressions.Regex(regexPattern, RegexOptions.IgnoreCase);
            return mods.Where(m => regex.Match(m.Name).Success);
        }

        public async Task<BeatModsEntry> GetLatestModByName(string name, string version = null) {
            var mods = await GetMods();
            return mods.FirstOrDefault(m => m.Name.ToLower() == name.ToLower());
        }
    }
}