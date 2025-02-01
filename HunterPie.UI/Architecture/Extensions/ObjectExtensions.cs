using HunterPie.Core.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HunterPie.UI.Architecture.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Converts a boxed string into thickness
    /// </summary>
    /// <param name="self">The string thickness formatted</param>
    /// <returns>Thickness</returns>
    public static Thickness ToThickness(this object self)
    {
        if (self is string str)
        {
            double[] values = str.Split(',')
                .Select(e => double.Parse(e))
                .ToArray();

            return new Thickness(values[0], values[1], values[2], values[3]);
        }

        throw new ArgumentException("argument must be of type string");
    }

    public static TOut CopyAs<TIn, TOut>(this TIn @object)
    {
        string serialized = JsonProvider.Serialize(@object);
        return JsonProvider.Deserialize<TOut>(serialized);
    }

    /// <summary>
    /// Applies values to T
    /// </summary>
    /// <typeparam name="T">The input type</typeparam>
    /// <param name="object">Object to apply properties to</param>
    /// <param name="block">Function that applies properties</param>
    public static T Apply<T>(this T @object, Action<T> block) where T : class
    {
        block(@object);

        return @object;
    }

    public static async Task<T> ApplyAsync<T>(this T @object, Func<T, Task> block) where T : class
    {
        await block(@object);

        return @object;
    }
}