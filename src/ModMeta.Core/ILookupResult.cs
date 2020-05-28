using System;
using System.Text.Json.Serialization;

namespace ModMeta.Core
{
    public interface ILookupResult
    {
        [JsonPropertyName("key")]
        string ResultId {get;set;}
        [JsonPropertyName("value")]
        IModInfo ModInfo {get;set;}
    }

    public interface IHashResult {
        string MD5Hash {get;set;}
        long Bytes {get;set;}
    }

    public interface IServer {
        string Url {get;set;}
        TimeSpan CacheDuration {get;set;}
    }
}