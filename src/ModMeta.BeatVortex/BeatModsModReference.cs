using ModMeta.Core;

namespace ModMeta.BeatVortex
{
    public class BeatModsModReference : IReference
    {
        public BeatModsModReference(string name, string version)
        {
            LogicalFileName = name;
            VersionMatch = $"^{version}";
        }
        public string FileMD5Hash => null;

        public long? FileSize => null;

        public string GameId => "beatsaber";

        public string VersionMatch {get;set;}

        public string LogicalFileName {get;set;}

        public string FileExpression {get;set;}
    }
}