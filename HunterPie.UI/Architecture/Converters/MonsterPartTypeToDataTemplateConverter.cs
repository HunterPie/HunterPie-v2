using HunterPie.Core.Game.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class MonsterPartTypeToDataTemplateConverter : IValueConverter
{
    public DataTemplate DefaultTemplate { get; set; }
    public DataTemplate SeverableTemplate { get; set; }
    public DataTemplate BreakableTemplate { get; set; }
    public DataTemplate QurioTemplate { get; set; }
    public DataTemplate Empty = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is PartType type
            ? type switch
            {
                PartType.Flinch => DefaultTemplate,
                PartType.Breakable => BreakableTemplate,
                PartType.Severable => SeverableTemplate,
                PartType.Qurio => QurioTemplate,
                PartType.Invalid => Empty,
                _ => throw new NotImplementedException(),
            }
            : null;//throw new ArgumentException("item must be a MonsterPartViewModel");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}