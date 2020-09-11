using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.InMemory;

namespace ModMeta.BeatVortex
{
    public interface IVersionProvider {
        Task<string> GetLatestVersion();
    }
    public class AliasesVersionProvider : IVersionProvider
    {
        private readonly HttpClient _versionClient;

        public AliasesVersionProvider(HttpClientFactory clientFactory)
        {
            this._versionClient = clientFactory.GetClient(new Uri("https://alias.beatmods.com/"));
        }
        public async Task<string> GetLatestVersion()
        {
            // Console.WriteLine("Getting versions from alias file");
            var url = "aliases.json";
            var str = await _versionClient.GetStringAsync(url);
            var aliases = JsonSerializer.Deserialize<Dictionary<string, IEnumerable<string>>>(str);
            return aliases.Keys.Last();
        }
    }

    public class BSIPAVersionProvider : IVersionProvider
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions options;

        public BSIPAVersionProvider(HttpClientFactory clientFactory, JsonSerializerOptions options)
        {
            this._client = clientFactory.GetCachedClient(new Uri("https://beatmods.com/api/v1/"));
            this.options = options;
        }
        public async Task<string> GetLatestVersion()
        {
            var url = "mod?name=BSIPA&status=approved&sort=updatedDate";
            var str = await _client.GetStringAsync(url);
            var mods = JsonSerializer.Deserialize<List<BeatModsEntry>>(str, options);
            var versions = mods.Select(m => m.GameVersion).Where(gv => Version.TryParse(gv, out _)).Select(v => Version.Parse(v)).OrderByDescending(v => v);
            var latest = versions.First().ToString();
            return versions.First().ToString();
        }
    }
}