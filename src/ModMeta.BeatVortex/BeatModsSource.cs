using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModMeta.Core;
using SemVer;
using Range = SemVer.Range;
using Version = SemVer.Version;

namespace ModMeta.BeatVortex
{
    public class BeatModsSource : IModMetaSource
    {
        private readonly BeatModsClient _client;
        private readonly ILogger<BeatModsSource> _logger;

        public BeatModsSource(BeatModsClient client, ILogger<BeatModsSource> logger)
        {
            _client = client;
            _logger = logger;
        }
        public bool IsCaching => false;

        public string DefaultGameId => "beatsaber";

        public LookupType SupportedTypes => LookupType.LogicalName|LookupType.FileExpression;

        private IEnumerable<ILookupResult> SortMatches(IEnumerable<BeatModsEntry> matches) {
            if (matches.Any()) {
                return matches.OrderByDescending(m => new Version(m.Version)).Select(m => new BeatModsLookupResult(m));
            } else {
                return new List<ILookupResult>();
            }
        }

        public async Task<IEnumerable<IModInfo>> GetAllMods()
        {
            var mods = await _client.GetAllMods();
            return mods.Select(m => m.ToModInfo());
        }

        public async Task<IEnumerable<ILookupResult>> GetByExpression(string fileExpression, VersionMatch versionMatch)
        {
            var mods = await _client.GetModsByPattern(fileExpression);
            var matches = mods.Where(m => m.MatchesVersion(versionMatch));
            return SortMatches(matches);
        }

        public Task<IEnumerable<ILookupResult>> GetByKey(string hashKey)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ILookupResult>> GetByLogicalName(string logicalName, VersionMatch versionMatch)
        {
            var mods = await _client.GetModsByName(logicalName);
            var matches = mods.Where(m => ((Range)versionMatch).IsSatisfied(m.Version));
            _logger.LogDebug($"Matched {matches.Count()}/{mods.Count()} mods from '{logicalName}'/'{versionMatch.ToString()}'");
            if (matches.Any()) {
                return matches.OrderByDescending(m => new Version(m.Version)).Select(m => new BeatModsLookupResult(m));
            } else {
                return new List<ILookupResult>();
            }
        }

        public Task<IEnumerable<ILookupResult>> GetByReference(IReference reference)
        {
            throw new NotImplementedException();
        }
    }
}
