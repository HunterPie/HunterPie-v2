using HunterPie.UI.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Main.Presentation;

internal class NavigationDataTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        return item switch
        {
            null => null,
            { } => NavigationProvider.FindBy(item.GetType())
        };
    }
}