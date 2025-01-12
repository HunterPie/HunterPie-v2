using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class OrientationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (Orientation)value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}