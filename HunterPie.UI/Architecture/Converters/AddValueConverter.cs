using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;
public class AddValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not int integer)
            throw new ArgumentException($"Expected {typeof(int)}, got {value?.GetType()}");

        return integer + 1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not int integer)
            throw new ArgumentException($"Expected {typeof(int)}, got {value?.GetType()}");

        return integer - 1;
    }
}