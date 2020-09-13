using System.Collections.Generic;

namespace ModMetaRelay
{
    public class RelayOptions
    {
        public List<string> PluginPaths {get;set;} = new List<string>();
        public int Timeout {get;set;} = 5000;
    }
}