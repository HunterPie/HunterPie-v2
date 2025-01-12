using HunterPie.Core.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

#nullable enable
namespace HunterPie.Core.Json;

public static class JsonProvider
{

    private static readonly JsonSerializerSettings DeserializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        TypeNameHandling = TypeNameHandling.Auto
    };

    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
    };

    private static readonly Lazy<JsonSerializerSettings> NewSerializeSettings = new(() =>
    {
        var serializer = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
        };
        serializer.Converters.Add(new StringEnumConverter());
        serializer.Converters.Add(new DurationConverter());

        return serializer;
    });

    public static void Populate(string value, object target) =>
        JsonConvert.PopulateObject(value, target, DeserializerSettings);

    public static string Serializer(object? value, bool indented = false) =>
        JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, NewSerializeSettings.Value);

    public static T Deserializer<T>(string value) =>
        JsonConvert.DeserializeObject<T>(value, NewSerializeSettings.Value)!;

    public static object Deserializer(string value, Type type) =>
        JsonConvert.DeserializeObject(value, type, NewSerializeSettings.Value)!;

    [Obsolete("This method is deprecated, use Serializer instead.")]
    public static string Serialize(object? value) =>
        JsonConvert.SerializeObject(value, Formatting.Indented, SerializerSettings);

    [Obsolete("This method is deprecated, use Deserializer instead.")]
    public static T Deserialize<T>(string value) =>
        JsonConvert.DeserializeObject<T>(value)!;
}
#nullable restore