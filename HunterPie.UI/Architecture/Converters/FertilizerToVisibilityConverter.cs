using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class FertilizerToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Fertilizer fertilizer)
            return Visibility.Collapsed;

        return fertilizer switch
        {
            Fertilizer.None => Visibility.Collapsed,
            _ => Visibility.Visible,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}