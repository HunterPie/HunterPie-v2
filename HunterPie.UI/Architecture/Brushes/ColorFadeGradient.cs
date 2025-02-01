using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Brushes;

public static class ColorFadeGradient
{
    private static readonly Color StartTransparency = Color.FromArgb(0xC0, 0x0, 0x0, 0x0);
    private static readonly Color EndTransparency = Color.FromArgb(0xFF, 0x0, 0x0, 0x0);

    public static LinearGradientBrush FromColor(Color color)
    {
        LinearGradientBrush brush = new()
        {
            StartPoint = new Point(1, 0),
            EndPoint = new Point(1, 1),
            GradientStops = new()
        {
            new(color - StartTransparency, 0),
            new(color - EndTransparency, 0.8)
        }
        };

        return brush;
    }
}