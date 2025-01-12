using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters.Brushes;

public class BrushOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is not double alpha)
            return value;

        if (value is not Brush brush)
            throw new ArgumentException($"{nameof(value)} must be of type {nameof(Brush)}");

        Brush clonedBrush = brush.Clone();
        clonedBrush.Opacity = alpha;
        clonedBrush.Freeze();

        return clonedBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}