using HunterPie.Core.Game.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters;

public class CrownToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string iconName = null;

        switch ((Crown)value)
        {
            case Crown.None:
                break;
            case Crown.Mini:
                iconName = "CROWN_MINI";
                break;
            case Crown.Silver:
                iconName = "CROWN_SILVER";
                break;
            case Crown.Gold:
                iconName = "CROWN_GOLD";
                break;
        }

        return iconName is null ? null : (object)(ImageSource)Application.Current.FindResource(iconName);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}