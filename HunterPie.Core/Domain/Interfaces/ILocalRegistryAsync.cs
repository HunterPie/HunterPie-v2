using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Interfaces;
public interface ILocalRegistryAsync
{
    public Task SetAsync<T>(string name, T value);
    public Task<bool> ExistsAsync(string name);
    public Task<object?> GetAsync(string name);
    public Task<T?> GetAsync<T>(string name);
    public Task DeleteAsync(string name);
}