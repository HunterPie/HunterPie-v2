using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class OrientationToOppositeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (Orientation)((int)value ^ 1);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}