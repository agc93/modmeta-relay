using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModMetaRelay
{
    public static class CoreExtensions
    {
        public static async Task<IEnumerable<T>> WhenAll<T>(this IEnumerable<Task<T>> tasks)
        {
            return await Task.WhenAll(tasks);
        }
    }
}