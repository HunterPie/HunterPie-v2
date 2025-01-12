using Newtonsoft.Json;
using System;

namespace HunterPie.Core.Json.Converters;

#nullable enable
public class DurationConverter : JsonConverter
{
    private const double NANOSECOND = 1e6;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not TimeSpan { } timespan)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteValue(timespan.TotalMilliseconds * NANOSECOND);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        long value = serializer.Deserialize<long>(reader);

        return TimeSpan.FromMilliseconds(value / NANOSECOND);
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
}