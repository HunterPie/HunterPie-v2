using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration(name: "METER_WIDGET_STRING",
    icon: "ICON_METER",
    group: CommonConfigurationGroups.OVERLAY,
    availableGames: GameProcessType.MonsterHunterWorld | GameProcessType.MonsterHunterRise)]
public class DamageMeterWidgetConfig : IWidgetSettings, ISettings
{
    [ConfigurationProperty("INITIALIZE_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.WIDGET)]
    [ConfigurationCondition]
    public Observable<bool> Initialize { get; set; } = true;

    [ConfigurationProperty("ENABLE_WIDGET_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> Enabled { get; set; } = true;

    [ConfigurationProperty("HIDE_WHEN_UI_VISIBLE_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> HideWhenUiOpen { get; set; } = false;

    [ConfigurationProperty("DAMAGE_METER_ENABLE_SHOULD_HIGHLIGHT_MYSELF", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<bool> ShouldHighlightMyself { get; set; } = false;

    [ConfigurationProperty("DAMAGE_METER_ENABLE_SHOULD_BLUR_NAMES", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<bool> ShouldBlurNames { get; set; } = false;

    [ConfigurationProperty("DAMAGE_METER_ENABLE_OTOMOS", group: CommonConfigurationGroups.CUSTOMIZATIONS, availableGames: GameProcessType.MonsterHunterRise)]
    public Observable<bool> ShouldShowOtomos { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_SHOULD_SHOW_DPS", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<bool> ShouldShowDPS { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_SHOULD_SHOW_DAMAGE", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<bool> ShouldShowDamage { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_DPS_CALCULATION_STRATEGY_STRING", group: CommonConfigurationGroups.WIDGET)]
    public Observable<DPSCalculationStrategy> DpsCalculationStrategy { get; set; } = DPSCalculationStrategy.RelativeToJoin;

    [ConfigurationProperty("DAMAGE_METER_DAMAGE_PLOT_STRATEGY_STRING", group: CommonConfigurationGroups.DAMAGE_PLOT)]
    public Observable<DamagePlotStrategy> DamagePlotStrategy { get; set; } = Enums.DamagePlotStrategy.DamagePerSecond;

    [ConfigurationProperty("DAMAGE_METER_ENABLE_DPS_PLOT", group: CommonConfigurationGroups.DAMAGE_PLOT)]
    public Observable<bool> ShouldShowPlots { get; set; } = true;

    [ConfigurationProperty("DAMAGE_METER_PLOT_LINE_SMOOTHING_STRING", group: CommonConfigurationGroups.DAMAGE_PLOT)]
    public Range PlotLineSmoothing { get; set; } = new Range(1.0, 1.0, 0.0, 0.01);

    [ConfigurationProperty("DAMAGE_METER_PLOT_LINE_THICKNESS_STRING", group: CommonConfigurationGroups.DAMAGE_PLOT)]
    public Range PlotLineThickness { get; set; } = new Range(2.0, 3.0, 0.2, 0.01);

    [ConfigurationProperty("DAMAGE_METER_SELF_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    public Color PlayerSelf { get; set; } = "#FF725AC1";

    [ConfigurationProperty("DAMAGE_METER_PLAYER_1_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    public Color PlayerFirst { get; set; } = "#FF6184D8";

    [ConfigurationProperty("DAMAGE_METER_PLAYER_2_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    public Color PlayerSecond { get; set; } = "#FF50C5B7";

    [ConfigurationProperty("DAMAGE_METER_PLAYER_3_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    public Color PlayerThird { get; set; } = "#FF9CEC5B";

    [ConfigurationProperty("DAMAGE_METER_PLAYER_4_COLOR_STRING", group: CommonConfigurationGroups.COLORS)]
    public Color PlayerFourth { get; set; } = "#FFF0F465";

    [ConfigurationProperty("DAMAGE_METER_NPC_COLOR_STRING", availableGames: GameProcessType.MonsterHunterRise, group: CommonConfigurationGroups.COLORS)]
    public Color NpcColor { get; set; } = "#FF9534EB";

    [ConfigurationProperty("WIDGET_POSITION", group: CommonConfigurationGroups.WIDGET)]
    public Position Position { get; set; } = new(0, 400);

    [ConfigurationProperty("WIDGET_OPACITY", group: CommonConfigurationGroups.WIDGET)]
    public Range Opacity { get; set; } = new(1, 1, 0, 0.1);

    [ConfigurationProperty("WIDGET_SCALE", group: CommonConfigurationGroups.WIDGET)]
    public Range Scale { get; set; } = new(1, 2, 0, 0.1);

    [ConfigurationProperty("ENABLE_STREAMER_MODE", group: CommonConfigurationGroups.WIDGET)]
    public Observable<bool> StreamerMode { get; set; } = false;
}