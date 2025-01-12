using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class SizeToDoubleSizeConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 3)
            return new Thickness(0, 0, 0, 0);

        double size = (double)values[0];
        double parentSize = (double)values[2];
        var parentMargin = (Thickness)values[1];

        double diff = size - parentSize;

        return new Thickness(
              parentMargin.Left - (diff / 2),
              parentMargin.Top,
              parentMargin.Right,
              parentMargin.Bottom
        );
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}