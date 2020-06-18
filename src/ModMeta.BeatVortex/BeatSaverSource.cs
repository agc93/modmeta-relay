using System.Collections.Generic;
using System.Threading.Tasks;
using BeatSaverSharp;
using ModMeta.Core;

namespace ModMeta.BeatVortex
{
    public class BeatSaverSource : IModMetaSource
    {
        private readonly BeatSaver _client;

        public BeatSaverSource()
        {
            var version = typeof(BeatSaverSource).Assembly.GetName().Version;
            var options = new HttpOptions()
            {
                ApplicationName = "ModMetaRelay",
                Version = version,
                HandleRateLimits = true,
                Timeout = System.TimeSpan.FromSeconds(4)
            };
            // Use this to interact with the API
            _client = new BeatSaver(options);
        }
        public bool IsCaching => false;

        public string DefaultGameId => "beatsaber";

        public LookupType SupportedTypes => LookupType.LogicalName|LookupType.FileExpression;

        public Task<IEnumerable<IModInfo>> GetAllMods()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<ILookupResult>> GetByExpression(string fileExpression, VersionMatch versionMatch)
        {
            if (string.IsNullOrWhiteSpace(fileExpression)) {
                throw new System.ArgumentNullException(nameof(fileExpression));
            }
            var fileName = System.IO.Path.GetFileNameWithoutExtension(fileExpression);
            if (fileName.Length == 40) {
                var map = await _client.Hash(fileName);
                return new List<ILookupResult> {
                    new BeatSaverLookupResult(map)
                };
            }
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<ILookupResult>> GetByKey(string hashKey)
        {
            if (hashKey.Length == 40) {
                var map = await _client.Hash(hashKey);
                return new List<ILookupResult> {
                    new BeatSaverLookupResult(map)
                };
            } else {
                throw new System.NotImplementedException();
            }
        }

        public async Task<IEnumerable<ILookupResult>> GetByLogicalName(string logicalName, VersionMatch versionMatch)
        {
            var map = await _client.Key(logicalName);
            return new List<ILookupResult> {
                new BeatSaverLookupResult(map)
            };
        }

        public async Task<IEnumerable<ILookupResult>> GetByReference(IReference reference)
        {
            if (reference.GameId == "beatsaber" && !string.IsNullOrWhiteSpace(reference.LogicalFileName)) {
                var map = await _client.Key(reference.LogicalFileName);
                return new List<ILookupResult> {
                    new BeatSaverLookupResult(map)
                };
            }
            if (reference.GameId == "beatsaber" && !string.IsNullOrWhiteSpace(reference.VersionMatch)) {
                var map = await _client.Hash(reference.VersionMatch);
                return new List<ILookupResult> {
                    new BeatSaverLookupResult(map)
                };
            }
            return new List<ILookupResult>();
        }
    }
}