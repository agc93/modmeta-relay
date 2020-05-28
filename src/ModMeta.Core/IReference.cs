namespace ModMeta.Core
{
    public interface IReference
    {
        string FileMD5Hash {get;}
        long? FileSize {get;}
        string GameId {get;}
        string VersionMatch {get;}
        string LogicalFileName {get;}
        string FileExpression {get;}
    }
}