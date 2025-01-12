using HunterPie.UI.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Main.Presentation;

public class MainBodyDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate? Default { get; init; }

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        return item switch
        {
            null => Default,
            _ => NavigationProvider.FindBy(item.GetType()),
        };
    }
}