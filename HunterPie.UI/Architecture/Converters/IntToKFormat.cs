using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class IntToKFormat : IValueConverter
{
    private readonly char[] _suffixes = { 'K', 'M', 'B', 'T', 'Q' };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double @decimal = value switch
        {
            int v => v,
            float v => v,
            double v => v,
            _ => throw new ArgumentException("value must be an integer")
        };
        int kSuffix = -1;
        bool isOverThousand = @decimal >= 1_000;
        while (@decimal >= 1_000)
        {
            @decimal /= 1_000;
            kSuffix++;
        }

        return isOverThousand ? $"{@decimal:0.0}{_suffixes[kSuffix]}" : @decimal;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}