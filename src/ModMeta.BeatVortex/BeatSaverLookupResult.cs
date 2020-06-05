using System;
using BeatSaverSharp;
using ModMeta.Core;

namespace ModMeta.BeatVortex
{
    public class BeatSaverLookupResult : ILookupResult
    {
        public BeatSaverLookupResult(Beatmap map)
        {
            ResultId = map.Hash;
            var info = new ModInfo {
                GameId = "beatsaber",
                FileVersion = map.Hash,
                // SourceUrl = new Uri($"https://beatmods.com/api/v1/mod/{entry.DocumentId}"),
                SourceUrl = new Uri($"https://beatmods.com{map.DownloadURL}"),
                Source = "beatsaver",
                FileName = System.IO.Path.GetFileName(map.DownloadURL),
                LogicalFileName = map.Key,
                Expires = DateTime.UtcNow.AddHours(24).Ticks,
                Details = new ModDetails {
                    Author = map.Metadata.LevelAuthorName ?? map.Uploader.Username,
                    Description = map.Description,
                    FileId = map.Key,
                    HomePage = $"https://beatsaver.com/beatmap/{map.Key}",
                }
            };
            ModInfo = info;
        }
        public string ResultId { get; set; }
        public IModInfo ModInfo { get; set; }
    }
}