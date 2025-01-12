using System;
using System.Globalization;
using System.Windows.Data;
using Converter = System.Convert;

namespace HunterPie.UI.Architecture.Converters;

public class TimerToSizeConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 3)
            throw new Exception("Expected at least 3 arguments");

        try
        {
            double timer = Converter.ToDouble(values[0]);
            double maxTimer = Math.Max(1.0, Converter.ToDouble(values[1]));
            double maxSize = Converter.ToDouble(values[2]);

            return Math.Max(0.0, maxSize * (timer / maxTimer));
        }
        catch
        {
            return 0.0;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}