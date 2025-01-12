using HunterPie.Core.Client.Localization;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class RiseMonsterAilmentIdToNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string id = (string)value;
        return Localization.Query($"//Strings/Ailments/Rise/Ailment[@Id='{id}']")?.Attributes["String"].Value
            ?? id;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}