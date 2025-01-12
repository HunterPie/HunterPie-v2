using HunterPie.Core.Crypto;
using HunterPie.Core.Settings.Types;
using HunterPie.DI;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace HunterPie.Core.Converters;

public class SecretConverter : JsonConverter
{
    private ICryptoService CryptoService => DependencyContainer.Get<ICryptoService>();

    public override bool CanConvert(Type objectType) => objectType.GetInterfaces().Contains(typeof(Secret));

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        string value = reader.Value != null
            ? (string)Convert.ChangeType(reader.Value, typeof(string))
            : string.Empty;

        objectType.GetProperty(nameof(Secret.EncryptedValue))?
            .SetValue(existingValue, value);

        return CryptoService.Decrypt(value);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        object? prop = value?
                .GetType()
                .GetProperty(nameof(Secret.EncryptedValue))?
                .GetValue(value);

        string secret = prop switch
        {
            string str => CryptoService.Encrypt(str),
            _ => string.Empty
        };

        serializer.Serialize(writer, secret);
    }
}