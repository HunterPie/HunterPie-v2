using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Core.Game.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Overlay.Widgets.Activities.Presentation;

public class ActivityTemplateSelector : DataTemplateSelector
{
    public DataTemplate SubmarineTemplate { get; set; }
    public DataTemplate TrainingDojoTemplate { get; set; }
    public DataTemplate MeowcenariesTemplate { get; set; }
    public DataTemplate CohootTemplate { get; set; }
    public DataTemplate HarvestBoxTemplate { get; set; }
    public DataTemplate SteamworksTemplate { get; set; }
    public DataTemplate ArgosyTemplate { get; set; }
    public DataTemplate TailraidersTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        return item is IActivity activity
            ? activity.Type switch
            {
                ActivityType.HarvestBox => HarvestBoxTemplate,
                ActivityType.Argosy => ArgosyTemplate,
                ActivityType.Submarine => SubmarineTemplate,
                ActivityType.Meowcenaries => MeowcenariesTemplate,
                ActivityType.TrainingDojo => TrainingDojoTemplate,
                ActivityType.Cohoot => CohootTemplate,
                ActivityType.Steamworks => SteamworksTemplate,
                ActivityType.Tailraiders => TailraidersTemplate,
                _ => throw new NotImplementedException($"Missing implementation for {nameof(activity.Type)}")
            }
            : throw new ArgumentException($"item must be an {nameof(IActivity)}");
    }
}