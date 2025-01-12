using System.Windows;

namespace HunterPie.UI.Settings;

public static class DataTemplateFactory
{
    public static DataTemplate Create<TView>()
    {
        FrameworkElementFactory factory = new(typeof(TView));
        return new DataTemplate { VisualTree = factory };
    }
}