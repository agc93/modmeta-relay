using System;
using System.Collections.Generic;

namespace ModMeta.Core
{
    public class ModInfo : IModInfo
    {
        public string FileName { get; set; }

        public long FileSize {get; set;}

        public string GameId {get; set;}

        public string LogicalFileName { get; set; }

        public string FileVersion { get; set; }

        public string FileMD5Hash { get; set; }

        public Uri SourceUrl { get; set; }

        public string Source { get; set; }

        public IList<IRule> Rules { get; set; }

        public long Expires { get; set; }

        public ModDetails Details { get; set; }
    }
}