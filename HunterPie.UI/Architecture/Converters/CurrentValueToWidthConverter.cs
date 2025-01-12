using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Converter = System.Convert;

namespace HunterPie.UI.Architecture.Converters;

public class CurrentValueToWidthConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            double currentValue = Converter.ToDouble(values[0]);
            double maxValue = Converter.ToDouble(values[1]);
            double maxWidth = Converter.ToDouble(values[2]);
            Thickness border = ((Thickness?)values.ElementAtOrDefault(3)) ?? new Thickness();
            double sides = border.Left + border.Right;

            double width = (maxWidth * (currentValue / Math.Max(maxValue, 1))) - sides;
            return Math.Max(1.0, width);
        }
        catch
        {
            return 1.0;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}