using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Classes.Presentation;

#nullable enable
public class LongSwordLevelColorConverter : IValueConverter
{
    public Brush? DefaultBrush { get; init; }
    public Brush? LevelOneBrush { get; init; }
    public Brush? LevelTwoBrush { get; init; }
    public Brush? LevelMaxBrush { get; init; }

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not int level)
            return DefaultBrush;

        return level switch
        {
            1 => LevelOneBrush,
            2 => LevelTwoBrush,
            3 => LevelMaxBrush,
            _ => DefaultBrush
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}