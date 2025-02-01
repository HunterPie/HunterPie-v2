using System.Windows;

namespace HunterPie.UI.Architecture.Utils;
public static class PointExtensions
{

    public static bool IsWithinBounds(this Point position, FrameworkElement component)
    {
        bool isNotNegative = position.Y >= 0 && position.X >= 0;
        bool isWithinBounds = position.Y < component.ActualHeight && position.X < component.ActualWidth;

        return isNotNegative && isWithinBounds;
    }
}