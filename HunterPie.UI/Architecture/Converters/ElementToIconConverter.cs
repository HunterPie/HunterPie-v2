using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class ElementToIconConverter : IValueConverter
{
    private const string FIRE = "ELEMENT_FIRE";
    private const string WATER = "ELEMENT_WATER";
    private const string THUNDER = "ELEMENT_THUNDER";
    private const string ICE = "ELEMENT_ICE";
    private const string DRAGON = "ELEMENT_DRAGON";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is Element element
            ? element switch
            {
                Element.Fire => Resources.Icon(FIRE),
                Element.Water => Resources.Icon(WATER),
                Element.Ice => Resources.Icon(ICE),
                Element.Thunder => Resources.Icon(THUNDER),
                Element.Dragon => Resources.Icon(DRAGON),
                _ => throw new NotImplementedException($"element {value} not a valid element")
            }
            : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}