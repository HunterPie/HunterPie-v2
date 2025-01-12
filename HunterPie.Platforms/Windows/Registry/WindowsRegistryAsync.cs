using HunterPie.Core.Domain.Interfaces;
using Microsoft.Win32;
using WinRegistry = Microsoft.Win32.Registry;

namespace HunterPie.Platforms.Windows.Registry;

internal class WindowsRegistryAsync : ILocalRegistryAsync, ILocalRegistry
{
    private readonly RegistryKey _key;

    public WindowsRegistryAsync()
    {
        _key = WinRegistry.CurrentUser.CreateSubKey(@"SOFTWARE\HunterPie");
    }

    public Task SetAsync<T>(string name, T value) =>
        Task.Run(() => Set(name, value));

    public Task<bool> ExistsAsync(string name) =>
        Task.Run(() => Exists(name));

    public Task<object?> GetAsync(string name) =>
        Task.Run(() => Get(name));

    public Task<T?> GetAsync<T>(string name) =>
        Task.Run(() => Get<T>(name));

    public Task DeleteAsync(string name) =>
        Task.Run(() => Delete(name));

    public void Set<T>(string name, T value) =>
        _key.SetValue(name, value);

    public bool Exists(string name) =>
        _key.GetValue(name) is not null;

    public object? Get(string name) =>
        _key.GetValue(name);

    public T? Get<T>(string name)
    {
        object? converted = Convert.ChangeType(_key.GetValue(name), typeof(T));

        if (converted is T value)
            return value;

        return default;
    }

    public void Delete(string name) =>
        _key.DeleteValue(name);
}