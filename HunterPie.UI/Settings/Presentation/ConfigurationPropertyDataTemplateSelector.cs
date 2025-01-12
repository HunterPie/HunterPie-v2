using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Settings.Presentation;

#nullable enable
public class ConfigurationPropertyDataTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        return item switch
        {
            null => null,
            { } => ConfigurationTemplateProvider.FindBy(item.GetType())
        };
    }
}