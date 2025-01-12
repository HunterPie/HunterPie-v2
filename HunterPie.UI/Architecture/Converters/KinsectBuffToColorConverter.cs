using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;


namespace HunterPie.UI.Architecture.Converters;

public class KinsectBuffToColorConverter : IValueConverter
{
    private static readonly KeyValuePair<KinsectBuff, Color>[] KinsectBuffColors =
    {
        new(KinsectBuff.Attack, Resources.Get<Color>("INSECT_GLAIVE_EXTRACT_ATTACK_COLOR")),
        new(KinsectBuff.Speed, Resources.Get<Color>("INSECT_GLAIVE_EXTRACT_SPEED_COLOR")),
        new(KinsectBuff.Defense, Resources.Get<Color>("INSECT_GLAIVE_EXTRACT_DEFENSE_COLOR")),
        new(KinsectBuff.Heal, Resources.Get<Color>("INSECT_GLAIVE_EXTRACT_HEAL_COLOR")),
        new(KinsectBuff.None, Resources.Get<Color>("INSECT_GLAIVE_EXTRACT_NONE_COLOR"))
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            KinsectBuff buff => KinsectBuffColors.First(it => it.Key == buff).Value,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}