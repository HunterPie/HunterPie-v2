using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class WeaponToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Weapon weap)
        {
            string iconName;

            switch (weap)
            {
                case Weapon.Greatsword:
                    iconName = "ICON_GREATSWORD";
                    break;
                case Weapon.SwordAndShield:
                    iconName = "ICON_SWORDANDSHIELD";
                    break;
                case Weapon.DualBlades:
                    iconName = "ICON_DUALBLADES";
                    break;
                case Weapon.Longsword:
                    iconName = "ICON_LONGSWORD";
                    break;
                case Weapon.Hammer:
                    iconName = "ICON_HAMMER";
                    break;
                case Weapon.HuntingHorn:
                    iconName = "ICON_HUNTINGHORN";
                    break;
                case Weapon.Lance:
                    iconName = "ICON_LANCE";
                    break;
                case Weapon.GunLance:
                    iconName = "ICON_GUNLANCE";
                    break;
                case Weapon.SwitchAxe:
                    iconName = "ICON_SWITCHAXE";
                    break;
                case Weapon.ChargeBlade:
                    iconName = "ICON_CHARGEBLADE";
                    break;
                case Weapon.InsectGlaive:
                    iconName = "ICON_INSECTGLAIVE";
                    break;
                case Weapon.Bow:
                    iconName = "ICON_BOW";
                    break;
                case Weapon.HeavyBowgun:
                    iconName = "ICON_HEAVYBOWGUN";
                    break;
                case Weapon.LightBowgun:
                    iconName = "ICON_LIGHTBOWGUN";
                    break;
                default:
                    return null;
            }

            return Resources.Icon(iconName);
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}