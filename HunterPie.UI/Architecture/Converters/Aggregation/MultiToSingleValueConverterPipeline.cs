using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters.Aggregation;

public class MultiToSingleValueConverterPipeline : List<IValueConverter>, IMultiValueConverter
{
    public required IMultiValueConverter Entrypoint { get; init; }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        object result = Entrypoint.Convert(values, targetType, parameter, culture);

        return this.Aggregate(
            seed: result,
            (current, next) => next.Convert(current, targetType, parameter, culture));
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}