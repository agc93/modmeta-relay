using System;
using System.Linq;
using System.Text.Json;
using ModMeta.Core;
using ModMeta.Core.Rules;
using Range = SemVer.Range;

namespace ModMeta.BeatVortex
{
    public static class BeatModsExtensions
    {
        internal static IModInfo ToModInfo(this BeatModsEntry entry) {
            var info = new ModInfo {
                GameId = "beatsaber",
                FileVersion = entry.Version,
                // SourceUrl = new Uri($"https://beatmods.com/api/v1/mod/{entry.DocumentId}"),
                SourceUrl = new Uri($"https://beatmods.com{entry.Downloads.First().Url}"),
                Source = "beatmods",
                FileName = System.IO.Path.GetFileName(entry.Downloads.First().Url),
                LogicalFileName = entry.Name,
                Expires = DateTime.UtcNow.AddHours(24).Ticks,
                Details = new ModDetails {
                    Author = entry.Author.UserName,
                    Category = entry.Category,
                    Description = entry.Description,
                    FileId = entry.DocumentId,
                    HomePage = entry.Link.ToString(),
                }
            };
            if (entry.Dependencies.Any(e => e.TryGetProperty("name", out _))) {
                info.Rules = entry.Dependencies.Select(e => JsonSerializer.Deserialize<BeatModsEntry>(e.GetRawText(), BeatModsClient.GetJsonOptions())).Select(d => {
                    return new BasicRule {Reference = new BeatModsModReference(d.Name, d.Version), Type = RuleType.Requires };
                }).Cast<IRule>().ToList();
            }
            return info;
        }

        internal static bool MatchesVersion(this BeatModsEntry m, VersionMatch version) {
            return ((Range)version).IsSatisfied(m.Version);
        }
    }
}