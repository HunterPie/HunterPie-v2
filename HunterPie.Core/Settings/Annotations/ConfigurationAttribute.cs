using HunterPie.Core.Domain.Enums;
using System;

namespace HunterPie.Core.Settings.Annotations;

#nullable enable

[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationAttribute : Attribute
{
    public string Name { get; init; }
    public string Icon { get; init; }
    public string? DependsOnFeature { get; init; }
    public GameProcessType AvailableGames { get; init; }

    public ConfigurationAttribute(
        string name,
        string icon,
        string? dependsOnFeature = null,
        GameProcessType availableGames = GameProcessType.All
    )
    {
        Name = name;
        Icon = icon;
        DependsOnFeature = dependsOnFeature;
        AvailableGames = availableGames;
    }
}