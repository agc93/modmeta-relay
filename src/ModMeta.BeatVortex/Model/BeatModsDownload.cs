using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ModMeta.BeatVortex
{
    public class BeatModsDownload {
        public string Type {get;set;}
        [JsonPropertyName("hashMd5")]
        public List<FileHash> Files {get;set;}
        public string Url {get;set;}
    }
}