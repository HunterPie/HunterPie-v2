using System;
using System.Collections.Generic;

namespace HunterPie.Core.Address.Map;

public interface IAddressMapParser
{
    public IReadOnlyDictionary<Type, Dictionary<string, object>> Items { get; }

    public IEnumerable<Type> Types => Items.Keys;

    /// <summary>
    /// Gets an existing key from the map
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <param name="key">Key to be looked up</param>
    /// <returns>Value casted to its type</returns>
    public T Get<T>(string key);

    /// <summary>
    /// Add a new value to the map lookup table
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="key">Name for the value</param>
    /// <param name="value">Value</param>
    public void Add<T>(string key, T value);
}