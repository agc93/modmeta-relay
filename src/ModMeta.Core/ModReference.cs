namespace ModMeta.Core
{
    public class ModReference : IReference
    {
        public ModReference(string gameId)
        {
            GameId = gameId;
        }

        public ModReference(string gameId, string logicalFileName, string versionMatch) : this(gameId)
        {
            LogicalFileName = logicalFileName;
            VersionMatch = versionMatch;
        }
        public string FileMD5Hash {get;set;}

        public long? FileSize {get;set;}

        public string GameId {get;}

        public string VersionMatch {get;set;}

        public string LogicalFileName {get;set;}

        public string FileExpression {get;set;}
    }
}