using HunterPie.Core.Architecture;
using HunterPie.Core.Converters;
using Newtonsoft.Json;

namespace HunterPie.Core.Settings.Types;

[JsonConverter(typeof(SecretConverter))]
public class Secret : Bindable
{
    private string _value = string.Empty;

    [JsonIgnore]
    public string Value
    {
        get => _value;
        set => SetValue(ref _value, value);
    }

    public string EncryptedValue
    {
        get => Value;
        set => Value = value;
    }

    public override string ToString() => Value;

    public static implicit operator string?(Secret? self) => self?.Value;
}