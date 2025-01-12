using HunterPie.Core.Client.Configuration.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

#nullable enable
public class GameTypeToGameAliasConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not GameType gameType)
            return "Unknown";

        return gameType switch
        {
            GameType.Rise => "MHR",
            GameType.World => "MHW",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}