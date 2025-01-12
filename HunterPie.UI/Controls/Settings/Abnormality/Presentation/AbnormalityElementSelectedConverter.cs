using HunterPie.Core.Architecture;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Controls.Settings.Abnormality.Presentation;

public class AbnormalityElementSelectedConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2)
            throw new ArgumentException("Expected at least 2 values");

        if (values[0] is not string abnormalityId
            || values[1] is not ObservableHashSet<string> enabledAbnormalities)
            throw new ArgumentException($"Expected string and {nameof(ObservableHashSet<string>)} but got {values[0].GetType()} and {values[1].GetType()}");

        return enabledAbnormalities.Contains(abnormalityId);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}