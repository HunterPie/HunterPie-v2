using HunterPie.Features.Settings.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Settings.Presentation;

internal class ConfigurationCategoryTemplateSelector : DataTemplateSelector
{
    public DataTemplate? TitleTemplate { get; set; }
    public DataTemplate? TabTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is not IConfigurationCategory category)
            return null;

        return category switch
        {
            ConfigurationCategoryTab => TabTemplate,
            ConfigurationCategoryTitle => TitleTemplate,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}