using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

#nullable enable
public class FertilizerToNameConverter : IValueConverter
{
    private static ILocalizationRepository LocalizationRepository =>
        DependencyContainer.Get<ILocalizationRepository>();

    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Fertilizer fertilizer)
            return null;

        string? localizationId = fertilizer switch
        {
            Fertilizer.None => "FERTILIZER_NONE_STRING",
            Fertilizer.PlantS => "FERTILIZER_PLANT_S_STRING",
            Fertilizer.PlantL => "FERTILIZER_PLANT_L_STRING",
            Fertilizer.FungiS => "FERTILIZER_FUNGI_S_STRING",
            Fertilizer.FungiL => "FERTILIZER_FUNGI_L_STRING",
            Fertilizer.HoneyS => "FERTILIZER_HONEY_S_STRING",
            Fertilizer.HoneyL => "FERTILIZER_HONEY_L_STRING",
            Fertilizer.GrowthS => "FERTILIZER_GROWTH_S_STRING",
            Fertilizer.GrowthL => "FERTILIZER_GROWTH_L_STRING",
            _ => null
        };

        return localizationId switch
        {
            { } => LocalizationRepository.FindStringBy($"//Strings/Fertilizers/Fertilizer[@Id='{localizationId}']"),
            _ => null
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}