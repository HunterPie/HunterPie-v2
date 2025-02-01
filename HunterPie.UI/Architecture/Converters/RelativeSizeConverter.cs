using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class RelativeSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double vl)
            return 0.0;

        bool success = double.TryParse(
            parameter.ToString(),
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out double percentage
        );

        if (!success)
            return 0.0;

        return vl * percentage;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}