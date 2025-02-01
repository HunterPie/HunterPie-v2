using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Chat.Presentation;

public class MemberIndexToColorConverter : IValueConverter
{
    private readonly SolidColorBrush[] _colors =
    {
        new SolidColorBrush(Color.FromArgb(0xFF, 0xED, 0x64, 0x91)),
        new SolidColorBrush(Color.FromArgb(0xFF, 0x64, 0xB6, 0xED)),
        new SolidColorBrush(Color.FromArgb(0xFF, 0xED, 0xAD, 0x64)),
        new SolidColorBrush(Color.FromArgb(0xFF, 0x64, 0xED, 0x99))
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is int index
            ? (object)_colors[index % _colors.Length]
            : throw new ArgumentException($"Expected int value, got {value.GetType()}");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}