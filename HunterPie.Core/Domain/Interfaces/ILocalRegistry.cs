namespace HunterPie.Core.Domain.Interfaces;

public interface ILocalRegistry
{
    public void Set<T>(string name, T value);
    public bool Exists(string name);
    public object? Get(string name);
    public T? Get<T>(string name);
    public void Delete(string name);
}