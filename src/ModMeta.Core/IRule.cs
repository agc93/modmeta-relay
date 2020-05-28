namespace ModMeta.Core
{
    public interface IRule
    {
        RuleType Type {get;}
        IReference Reference {get;}
        string Comment {get;}
    }
}