using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class PrefixFilterConverter : IValueConverter
{
    public string Prefix { get; init; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string str)
            return value;

        return str.StartsWith(Prefix)
            ? str[Prefix.Length..str.Length]
            : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}