using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class ReverseSecondsToTimeStringConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        string timeFormat = "mm\\:ss";
        if (values[0] is double val)
        {
            double maxVal = (double)values[1];
            if (maxVal - val > TimeSpan.MaxValue.TotalSeconds)
                return string.Empty;

            var span = TimeSpan.FromSeconds(maxVal - val);

            if (parameter is string format)
                timeFormat = format;

            return span.ToString(timeFormat);
        }

        return string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}