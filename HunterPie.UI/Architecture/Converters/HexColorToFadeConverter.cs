using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters;

/// <summary>
/// Converts a Hex Color string (e.g: #FF000102) into a fade
/// it's used by the <seealso cref="Overlay.Widgets.Damage.View.PlayerDamageView"/>
/// </summary>
public class HexColorToFadeConverter : IValueConverter
{

    private static readonly LinearGradientBrush _defaultBrush = new(new GradientStopCollection()
    {
        new GradientStop(Color.FromArgb(0, 0, 0, 0), 0),
        new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 1),
        new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 0.958),
        new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.955),
        new GradientStop(Color.FromArgb(0x50, 0xFF, 0xFF, 0xFF), 0.952),
    }, new Point(0.5, 0), new Point(0.5, 1));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string val)
        {
            var baseBrush = (Color)ColorConverter.ConvertFromString(val);
            Color fadeBrush = baseBrush - Color.FromArgb(0xAF, 0, 0, 0);

            LinearGradientBrush gradientBrush = new()
            {
                EndPoint = new Point(0.5, 1),
                StartPoint = new Point(0.5, 0)
            };

            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0));
            gradientBrush.GradientStops.Add(new GradientStop(baseBrush, 1));
            gradientBrush.GradientStops.Add(new GradientStop(baseBrush, 0.958));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.955));
            gradientBrush.GradientStops.Add(new GradientStop(fadeBrush, 0.952));

            return gradientBrush;
        }

        return _defaultBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}