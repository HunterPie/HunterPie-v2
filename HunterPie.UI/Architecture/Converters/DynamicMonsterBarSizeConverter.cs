using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class DynamicMonsterBarSizeConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is not double)
            return 0;

        if (values[1] is not int)
            return 0;

        double minWidth = (double)values[0];
        int monstersVisible = (int)values[1];

        double dynamicSize = minWidth + ((3 - monstersVisible) * (minWidth * 0.25));
        return dynamicSize;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}