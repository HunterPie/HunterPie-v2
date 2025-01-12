using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration("ACTIVITIES_WIDGET_STRING", "ICON_ARGOSY", availableGames: GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld)]
public class ActivitiesWidgetConfig : IWidgetSettings, ISettings
{
    #region General Settings
    [ConfigurationProperty("INITIALIZE_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.GENERAL)]
    [ConfigurationCondition]
    public Observable<bool> Initialize { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.GENERAL)]
    public Observable<bool> Enabled { get; set; } = true;

    [ConfigurationProperty("HIDE_WHEN_UI_VISIBLE_STRING", group: CommonConfigurationGroups.GENERAL)]
    public Observable<bool> HideWhenUiOpen { get; set; } = false;

    [ConfigurationProperty("WIDGET_OPACITY", group: CommonConfigurationGroups.GENERAL)]
    public Range Opacity { get; set; } = new(1, 1, 0.1, 0.1);

    [ConfigurationProperty("WIDGET_SCALE", group: CommonConfigurationGroups.GENERAL)]
    public Range Scale { get; set; } = new(1, 2, 0.1, 0.1);

    [ConfigurationProperty("ENABLE_STREAMER_MODE", group: CommonConfigurationGroups.GENERAL)]
    public Observable<bool> StreamerMode { get; set; } = false;

    [ConfigurationProperty("WIDGET_POSITION", group: CommonConfigurationGroups.GENERAL)]
    public Position Position { get; set; } = new(5, 935);
    #endregion

    #region Activity Settings
    [ConfigurationProperty("ACTIVITIES_ENABLE_ARGOSY_STRING", group: CommonConfigurationGroups.ACTIVITIES)]
    public Observable<bool> IsArgosyEnabled { get; set; } = true;

    [ConfigurationProperty("ACTIVITIES_ENABLE_TRAINING_DOJO_STRING", availableGames: GameProcessType.MonsterHunterRise, group: CommonConfigurationGroups.ACTIVITIES)]
    public Observable<bool> IsTrainingDojoEnabled { get; set; } = true;

    [ConfigurationProperty("ACTIVITIES_ENABLE_MEOWMASTERS_STRING", group: CommonConfigurationGroups.ACTIVITIES)]
    public Observable<bool> IsMeowmastersEnabled { get; set; } = true;

    [ConfigurationProperty("ACTIVITIES_ENABLE_COHOOT_STRING", availableGames: GameProcessType.MonsterHunterRise, group: CommonConfigurationGroups.ACTIVITIES)]
    public Observable<bool> IsCohootEnabled { get; set; } = true;

    [ConfigurationProperty("ACTIVITIES_ENABLE_HARVEST_BOX_STRING", availableGames: GameProcessType.MonsterHunterWorld, group: CommonConfigurationGroups.ACTIVITIES)]
    public Observable<bool> IsHarvestBoxEnabled { get; set; } = true;

    [ConfigurationProperty("ACTIVITIES_ENABLE_STEAMWORKS_STRING", availableGames: GameProcessType.MonsterHunterWorld, group: CommonConfigurationGroups.ACTIVITIES)]
    public Observable<bool> IsSteamworksEnabled { get; set; } = true;
    #endregion

}