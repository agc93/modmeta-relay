using ModMeta.Core;

namespace ModMeta.BeatVortex
{
    public class BeatModsLookupResult : ILookupResult
    {
        public BeatModsLookupResult(BeatModsEntry e)
        {
            ResultId = e.DocumentId;
            ModInfo = e.ToModInfo();
        }
        public string ResultId { get;set;}
        public IModInfo ModInfo { get;set;}
    }
}