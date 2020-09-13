using System;

namespace ModMeta.Core
{
    public enum RuleType
    {
        Before,
        After,
        Requires,
        Conflicts,
        Recommends,
        Provides
    }

    public class VersionMatch {
        public VersionMatch(string versionMatch)
        {
            rawMatch = versionMatch;
        }
        private string rawMatch;
        public static implicit operator VersionMatch(string s) {
            return new VersionMatch(s);
        }

        public static implicit operator string(VersionMatch vm) {
            return vm.rawMatch;
        }

        public static explicit operator SemVer.Range(VersionMatch vm) {
            return new SemVer.Range(vm.rawMatch);
        }

        public override string ToString()
        {
            return rawMatch;
        }
    }
}
