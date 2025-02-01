using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Utils;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;
public class WeaponToMeleeVisibility : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Weapon weapon)
        {
            return weapon.IsMelee() ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}