using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MediaBrushes = System.Windows.Media.Brushes;

namespace HunterPie.UI.Architecture.Converters;
public class ColorToBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush Default = MediaBrushes.White;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Color color)
            return Default;

        var brush = new SolidColorBrush(color);
        brush.Freeze();

        return brush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}