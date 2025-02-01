using HunterPie.Features.Settings.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Settings.Presentation;

internal class UpdateStatusTemplateSelector : DataTemplateSelector
{
    public DataTemplate? LoadingTemplate { get; set; }
    public DataTemplate? LatestTemplate { get; set; }
    public DataTemplate? NeedsUpdateTemplate { get; set; }
    public DataTemplate? ErrorTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject parentItemsControl)
    {
        if (item is not UpdateFetchStatus status)
            return null;

        return (status) switch
        {
            UpdateFetchStatus.Fetching => LoadingTemplate,
            UpdateFetchStatus.Error => ErrorTemplate,
            UpdateFetchStatus.Latest => LatestTemplate,
            UpdateFetchStatus.NeedsUpdate => NeedsUpdateTemplate,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}