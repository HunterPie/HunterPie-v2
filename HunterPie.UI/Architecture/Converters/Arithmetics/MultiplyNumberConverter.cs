using System;
using System.Globalization;
using System.Windows.Data;
using Converter = System.Convert;

namespace HunterPie.UI.Architecture.Converters.Arithmetics;

public class MultiplyNumberConverter : IValueConverter
{
    public required double Factor { get; init; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double convertedValue = Converter.ToDouble(value);

        return convertedValue * Factor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}