using HunterPie.Core.Domain.Cache.Model;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Cache;

public interface IAsyncCache
{
    public Task<T?> GetAsync<T>(string key);
    public Task SetAsync<T>(string key, T value, CacheOptions? options = null) where T : notnull;
    public Task ClearAsync(string key);
}