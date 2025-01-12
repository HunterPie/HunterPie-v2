using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class WirebugCooldownBorderConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 4)
            return 0.0;

        if (values[0] is not double cooldown
            || values[1] is not double maxCooldown
            || values[2] is not bool isAvailable
            || values[3] is not bool onCooldown)
            return 0.0;

        if (isAvailable && !onCooldown)
            return 1.0;

        return 1 - (cooldown / Math.Max(1.0, maxCooldown));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}