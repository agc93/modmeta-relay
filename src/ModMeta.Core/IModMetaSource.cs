using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ModMeta.Core
{
    [Flags]
    public enum LookupType {
        Hash = 1<<0,
        LogicalName = 1<<1,
        FileExpression = 1<<2,
        Reference = 1<<3
    }
    public interface IModMetaSource {
        bool IsCaching {get;}
        string DefaultGameId {get;}
        LookupType SupportedTypes {get;}
        Task<IEnumerable<ILookupResult>> GetByReference(IReference reference);
        Task<IEnumerable<ILookupResult>> GetByKey(string hashKey, string gameId = null);
        Task<IEnumerable<ILookupResult>> GetByLogicalName(string logicalName, VersionMatch versionMatch);
        Task<IEnumerable<ILookupResult>> GetByExpression(string fileExpression, VersionMatch versionMatch);
        Task<IEnumerable<IModInfo>> GetAllMods();
    }

    public interface IModMetaSourceFactory {
        IServiceCollection ConfigureServices(IServiceCollection services);
    }
}