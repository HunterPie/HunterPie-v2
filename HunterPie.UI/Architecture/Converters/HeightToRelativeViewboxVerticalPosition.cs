using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class HeightToRelativeViewboxVerticalPosition : IMultiValueConverter
{

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            throw new ArgumentException("expected at least 2 values");

        double height = (double)values[0];
        double maxHeight = (double)values[1];
        double ratio = maxHeight == height ? 1 : (maxHeight - height) / maxHeight;
        return new Rect(0, ratio, 1, 1);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}