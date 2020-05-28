using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ModMeta.BeatVortex
{
    internal class DependencyConverter : System.Text.Json.Serialization.JsonConverter<List<BeatModsEntry>>
    {
        public override List<BeatModsEntry> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return JsonSerializer.Deserialize<List<BeatModsEntry>>(reader.GetString(), options);
            }
            catch
            {
                return new List<BeatModsEntry>();
            }
        }

        public override void Write(Utf8JsonWriter writer, List<BeatModsEntry> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}