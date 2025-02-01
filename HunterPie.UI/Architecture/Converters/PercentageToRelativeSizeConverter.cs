using System;
using System.Globalization;
using System.Windows.Data;
using Converter = System.Convert;

namespace HunterPie.UI.Architecture.Converters;

/// <summary>
/// Returns the value for (value / maxValue) * size
/// </summary>
public class PercentageToRelativeSizeConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (values.Length < 3)
                throw new ArgumentException("there must be 3 double values");

            double current = Converter.ToDouble(values[0]);
            double max = Math.Max(1, Converter.ToDouble(values[1]));
            double relativeSize = Converter.ToDouble(values[2]);

            double result = current / max * relativeSize;

            return result < 0 ? 0 : result;
        }
        catch (Exception)
        {
            return 0.0;
        }
    }
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}