using HunterPie.Core.Client.Configuration.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class GameTypeToGameNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (GameType)value switch
        {
            GameType.Rise => "Monster Hunter Rise",
            GameType.World => "Monster Hunter: World",
            _ => "unknown"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}