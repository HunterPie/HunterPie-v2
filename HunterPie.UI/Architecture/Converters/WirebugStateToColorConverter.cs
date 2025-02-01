using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class WirebugStateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not WirebugState state)
            throw new ArgumentException($"Argument must be of type {nameof(WirebugState)} but got {value?.GetType()}");

        string color = "#FF64BAAC";

        switch (state)
        {
            case WirebugState.IceBlight:
                color = "#FF70D0F7";
                break;
            case WirebugState.WindMantle:
                color = "#FF00D600";
                break;
            case WirebugState.RubyWirebug:
                color = "#FFFE4A0D";
                break;
            case WirebugState.GoldWirebug:
                color = "#FFFFE21D";
                break;
            default:
                return color;
        }

        return color;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}