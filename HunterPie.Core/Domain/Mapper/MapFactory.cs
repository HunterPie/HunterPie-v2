using HunterPie.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace HunterPie.Core.Domain.Mapper;

/// <summary>
/// HunterPie's Map Factory for parsing objects into others
/// easily.
/// </summary>
public static class MapFactory
{
    private static readonly Dictionary<(Type, Type), object> Mappers = new();

    /// <summary>
    /// Adds a new IMapper to the MapFactory
    /// </summary>
    /// <typeparam name="TIn">Type to map from</typeparam>
    /// <typeparam name="TOut">Type to map to</typeparam>
    /// <param name="mapper">Mapper implementation</param>
    public static void Add<TIn, TOut>(IMapper<TIn, TOut> mapper)
    {
        Type typeIn = typeof(TIn);
        Type typeOut = typeof(TOut);
        (Type, Type) key = (typeIn, typeOut);

        if (Mappers.ContainsKey(key))
            throw new Exception($"There is already a mapper for {typeIn.Name} to {typeOut.Name}");

        Mappers[key] = mapper;
    }

    /// <summary>
    /// Find the mapper for a certain object into another and returns the mapped
    /// result
    /// </summary>
    /// <typeparam name="TIn">Type to map from</typeparam>
    /// <typeparam name="TOut">Type to map to</typeparam>
    /// <param name="object">Object to be mapped</param>
    /// <returns>Mapped object</returns>
    public static TOut Map<TIn, TOut>(TIn @object)
    {
        Type typeIn = typeof(TIn);
        Type typeOut = typeof(TOut);
        (Type, Type) key = (typeIn, typeOut);

        if (!Mappers.ContainsKey(key))
            throw new Exception($"Mapper for {typeIn.Name} to {typeOut.Name} not registered");

        var mapper = (IMapper<TIn, TOut>)Mappers[key];

        return mapper.Map(@object);
    }
}