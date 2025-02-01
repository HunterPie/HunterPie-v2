using HunterPie.Core.Converters;
using Newtonsoft.Json;

namespace HunterPie.Core.Architecture;

[JsonConverter(typeof(ObservableConverter))]
public class Observable<T> : Bindable
{
    private T _value;

    [JsonProperty]
    public T Value
    {
        get => _value;
        set => SetValue(ref _value, value);
    }

    [JsonConstructor]
    public Observable(T value)
    {
        Value = value;
    }

    public static implicit operator Observable<T>(T v) => new(v);

    public static implicit operator T(Observable<T> v) => v.Value;

    public static Observable<T> Default() => new(default);
}