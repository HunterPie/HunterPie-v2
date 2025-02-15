using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

#nullable enable
public class FertilizerToIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Fertilizer fertilizer)
            return null;

        string? iconName = fertilizer switch
        {
            Fertilizer.None => "ICON_WARN",
            Fertilizer.PlantS => "ICON_FERTILIZER_PLANT_S",
            Fertilizer.PlantL => "ICON_FERTILIZER_PLANT_L",
            Fertilizer.FungiS => "ICON_FERTILIZER_FUNGI_S",
            Fertilizer.FungiL => "ICON_FERTILIZER_FUNGI_L",
            Fertilizer.HoneyS => "ICON_FERTILIZER_HONEY_S",
            Fertilizer.HoneyL => "ICON_FERTILIZER_HONEY_L",
            Fertilizer.GrowthS => "ICON_FERTILIZER_GROWTH_S",
            Fertilizer.GrowthL => "ICON_FERTILIZER_GROWTH_L",
            _ => null
        };

        return iconName is null
            ? null
            : Resources.Icon(iconName);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}