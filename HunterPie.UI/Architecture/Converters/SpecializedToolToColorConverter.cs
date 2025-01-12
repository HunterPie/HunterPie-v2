using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters;

public class SpecializedToolToColorConverter : IValueConverter
{
    private static readonly Brush DefaultColor = new SolidColorBrush(Colors.White);

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is SpecializedToolType tool
            ? (object)(tool switch
            {
                SpecializedToolType.None => DefaultColor,
                SpecializedToolType.GhillieMantle => Resources.Get<Brush>("COLOR_GHILLIE_MANTLE"),
                SpecializedToolType.TemporalMantle => Resources.Get<Brush>("COLOR_TEMPORAL_MANTLE"),
                SpecializedToolType.HealthBooster => Resources.Get<Brush>("COLOR_HEALTH_BOOSTER"),
                SpecializedToolType.RocksteadyMantle => Resources.Get<Brush>("COLOR_ROCKSTEADY_MANTLE"),
                SpecializedToolType.ChallengerMantle => Resources.Get<Brush>("COLOR_CHALLENGER_MANTLE"),
                SpecializedToolType.VitalityMantle => Resources.Get<Brush>("COLOR_VITALITY_MANTLE"),
                SpecializedToolType.FireproofMantle => Resources.Get<Brush>("COLOR_FIREPROOF_MANTLE"),
                SpecializedToolType.WaterproofMantle => Resources.Get<Brush>("COLOR_WATERPROOF_MANTLE"),
                SpecializedToolType.IceproofMantle => Resources.Get<Brush>("COLOR_ICEPROOF_MANTLE"),
                SpecializedToolType.ThunderproofMantle => Resources.Get<Brush>("COLOR_THUNDERPROOF_MANTLE"),
                SpecializedToolType.DragonproofMantle => Resources.Get<Brush>("COLOR_DRAGONPROOF_MANTLE"),
                SpecializedToolType.CleanserBooster => Resources.Get<Brush>("COLOR_CLEANSER_BOOSTER"),
                SpecializedToolType.GliderMantle => Resources.Get<Brush>("COLOR_GLIDER_MANTLE"),
                SpecializedToolType.EvasionMantle => Resources.Get<Brush>("COLOR_EVASION_MANTLE"),
                SpecializedToolType.ImpactMantle => Resources.Get<Brush>("COLOR_IMPACT_MANTLE"),
                SpecializedToolType.ApothecaryMantle => Resources.Get<Brush>("COLOR_APOTHECARY_MANTLE"),
                SpecializedToolType.ImmunityMantle => Resources.Get<Brush>("COLOR_IMMUNITY_MANTLE"),
                SpecializedToolType.AffinityBooster => Resources.Get<Brush>("COLOR_AFFINITY_BOOSTER"),
                SpecializedToolType.BanditMantle => Resources.Get<Brush>("COLOR_BANDIT_MANTLE"),
                SpecializedToolType.AssassinsHood => Resources.Get<Brush>("COLOR_ASSASSINS_HOOD"),
                _ => throw new NotImplementedException(),
            })
            : throw new ArgumentException($"expected value to be of type {nameof(SpecializedToolType)}");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}