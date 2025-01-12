using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class NumberToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            float f => f != 0.0f,
            double d => d != 0.0f,
            int i => i != 0,
            long l => l != 0.0f,
            _ => throw new NotSupportedException($"value must be a number type, got {value?.GetType()}")
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}