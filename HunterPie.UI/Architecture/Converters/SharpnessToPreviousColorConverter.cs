﻿using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters;
public class SharpnessToPreviousColorConverter : IValueConverter
{

    private readonly Dictionary<Sharpness, Color> _associatedColors = new Dictionary<Sharpness, Color>(7)
    {
        { Sharpness.Broken, Resources.Get<Color>("WIDGET_SHARPNESS_BROKEN") },
        { Sharpness.Red, Resources.Get<Color>("WIDGET_SHARPNESS_RED") },
        { Sharpness.Orange, Resources.Get<Color>("WIDGET_SHARPNESS_ORANGE") },
        { Sharpness.Yellow, Resources.Get<Color>("WIDGET_SHARPNESS_YELLOW") },
        { Sharpness.Green, Resources.Get<Color>("WIDGET_SHARPNESS_GREEN") },
        { Sharpness.Blue, Resources.Get<Color>("WIDGET_SHARPNESS_BLUE") },
        { Sharpness.White, Resources.Get<Color>("WIDGET_SHARPNESS_WHITE") },
        { Sharpness.Purple, Resources.Get<Color>("WIDGET_SHARPNESS_PURPLE") },
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Sharpness sharpness && parameter is bool shouldConvert)
        {
            Color color = _associatedColors[sharpness - 1];

            return shouldConvert ? new SolidColorBrush(color) : color;
        }

        throw new ArgumentException("value must be sharpness");
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}