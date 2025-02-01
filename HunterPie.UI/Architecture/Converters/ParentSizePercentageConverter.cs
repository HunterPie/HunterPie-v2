using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class ParentSizePercentageConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double divisor = 100;
        _ = parameter is not null && double.TryParse(parameter.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out divisor);

        if (values[0] is not double ||
            values[1] is not double)
        {
            return 0.0;
        }

        double parentSize = (double)values[0];
        double percentage = (double)values[1] / divisor;

        return parentSize * percentage;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}