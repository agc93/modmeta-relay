using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ModMeta.Core
{
    public interface IModInfo
    {
        string FileName { get; }
        [JsonPropertyName("fileSizeBytes")]
        long FileSize { get; }
        string GameId { get; }
        string LogicalFileName { get; }
        string FileVersion { get; }
        [JsonPropertyName("fileMD5")]
        string FileMD5Hash { get; }
        [JsonPropertyName("sourceURI")]
        Uri SourceUrl { get; }
        string Source { get; }
        IList<IRule> Rules { get; }
        long Expires { get; }
        ModDetails Details { get; }
    }
}