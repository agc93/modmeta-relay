namespace ModMeta.Core.Rules
{
    public class BasicRule : IRule
    {
        public RuleType Type {get;set;}

        public IReference Reference {get;set;}

        public string Comment {get;set;}
    }
}