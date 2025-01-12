using HunterPie.Core.Domain.Cache.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Cache;

/// <summary>
/// A thread safe and asynchronous in-memory cache
/// </summary>
public class InMemoryAsyncCache : IAsyncCache
{
    private readonly SemaphoreSlim _semaphore = new(1);
    private readonly Dictionary<string, ThreadSafeCacheEntry> _innerCache = new(50);

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            await _semaphore.WaitAsync();

            bool exists = _innerCache.TryGetValue(key, out ThreadSafeCacheEntry? value);

            if (!exists || value is null)
                return default;

            if (value.ExpiresAt < DateTime.UtcNow)
                return default;

            return value.Value is T typedValue ? typedValue : default;
        }
        catch
        {
            return default;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SetAsync<T>(string key, T value, CacheOptions? options = null) where T : notnull
    {
        options ??= CacheOptions.Default;

        try
        {
            await _semaphore.WaitAsync();

            _innerCache[key] = new ThreadSafeCacheEntry(
                ExpiresAt: options.Ttl == TimeSpan.MaxValue
                    ? DateTime.MaxValue
                    : DateTime.UtcNow + options.Ttl,
                Value: value
            );
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ClearAsync(string key)
    {
        try
        {
            await _semaphore.WaitAsync();

            _innerCache.Remove(key);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}