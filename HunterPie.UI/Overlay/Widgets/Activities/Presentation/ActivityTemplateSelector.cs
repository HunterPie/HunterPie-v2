using HunterPie.UI.Overlay.Widgets.Activities.Common;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Overlay.Widgets.Activities.Presentation;

#nullable enable
public class ActivityTemplateSelector : DataTemplateSelector
{
    public required DataTemplate MonsterHunterWorldTemplate { get; init; }
    public required DataTemplate MonsterHunterRiseTemplate { get; init; }

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        return item is IActivitiesViewModel viewModel
            ? viewModel switch
            {
                MHWorldActivitiesViewModel => MonsterHunterWorldTemplate,
                MHRiseActivitiesViewModel => MonsterHunterRiseTemplate,
                _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
            }
            : null;
    }
}