using HunterPie.Core.Architecture;

namespace HunterPie.Core.Settings.Types;

public class Color : Bindable
{
    private string _value;

    public string Value { get => _value; set => SetValue(ref _value, value); }

    public Color(string value)
    {
        Value = value;
    }

    public static implicit operator Color(string v) => new(v);

    public static implicit operator string(Color v) => v.Value;
}