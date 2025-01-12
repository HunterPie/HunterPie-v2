using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters.Conditionals;

/// <summary>
/// Compares two objects and whether the first value is less than or equal to the second one
/// </summary>
public class LessThanOrEqualConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            throw new ArgumentException("Expected at least 2 arguments");

        if (values[0] is not IComparable first
            || values[1] is not IComparable second)
            return false;

        return first.CompareTo(second) <= 0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}