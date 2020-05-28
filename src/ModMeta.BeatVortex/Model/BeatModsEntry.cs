using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModMeta.BeatVortex
{
    public class BeatModsEntry {
        [JsonPropertyName("_id")]
        public string DocumentId {get;set;}
        public string Name {get;set;}
        public string Version {get;set;}
        public string GameVersion {get;set;}
        public BeatModsAuthor Author {get;set;}
        public BeatModsStatus Status {get;set;}
        public string Description {get;set;}
        public Uri Link {get;set;}
        public string Category {get;set;}
        public List<BeatModsDownload> Downloads {get;set;}
        public bool Required {get;set;}
        // [JsonConverter(typeof(DependencyConverter))]
        public List<JsonElement> Dependencies {get;set;}
    }
}