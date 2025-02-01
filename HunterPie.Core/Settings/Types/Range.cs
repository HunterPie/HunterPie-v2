using HunterPie.Core.Architecture;
using HunterPie.Core.Converters;
using Newtonsoft.Json;
using System;

namespace HunterPie.Core.Settings.Types;

[JsonConverter(typeof(RangeConverter))]
public class Range : Bindable
{
    private double _current;
    public double Current { get => _current; set => SetValue(ref _current, Math.Max(Math.Min(value, Max), Min)); }

    private double _max;
    public double Max { get => _max; set => SetValue(ref _max, value); }

    private double _min;
    public double Min { get => _min; set => SetValue(ref _min, value); }

    private double _step;
    public double Step { get => _step; set => SetValue(ref _step, value); }

    [JsonConstructor]
    public Range() { }

    public Range(double current, double max, double min, double step)
    {
        Max = max;
        Min = min;
        Step = step;
        Current = current;
    }
}