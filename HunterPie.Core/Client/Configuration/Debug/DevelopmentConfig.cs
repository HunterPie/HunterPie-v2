using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using System.Diagnostics;

namespace HunterPie.Core.Client.Configuration.Debug;

[Configuration(name: "DEV_TOOLS_STRING",
    icon: "ICON_BUG",
    group: CommonConfigurationGroups.CLIENT,
    dependsOnFeature: FeatureFlags.FEATURE_ADVANCED_DEV)]
public class DevelopmentConfig : ISettings
{
    #region Mocking Settings
    [ConfigurationProperty("DEV_MOCK_MONSTER_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockBossesWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_DAMAGE_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockDamageWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_ABNORMALITY_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockAbnormalityWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_WIREBUG_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockWirebugWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_ACTIVITIES_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockActivitiesWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_CHAT_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockChatWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_SPECIALIZED_TOOL_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockSpecializedToolWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_PLAYER_HUD_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockPlayerHudWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_INSECT_GLAIVE_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockInsectGlaiveWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_CHARGE_BLADE_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockChargeBladeWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_SWITCH_AXE_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockSwitchAxeWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_LONG_SWORD_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockLongSwordWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_DUAL_BLADES_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockDualBladesWidget { get; set; } = false;

    [ConfigurationProperty("DEV_MOCK_CLOCK_WIDGET_STRING", requiresRestart: true, group: CommonConfigurationGroups.MOCKS)]
    public Observable<bool> MockClockWidget { get; set; } = false;
    #endregion

    #region Development Settings
    [ConfigurationProperty("DEV_POOGIE_API_HOST_STRING", group: CommonConfigurationGroups.DEVELOPMENT)]
    public Observable<string> PoogieApiHost { get; set; } = "";
    #endregion

    #region Logging Settings
    [ConfigurationProperty("DEV_LOG_LEVEL", group: CommonConfigurationGroups.LOGGING)]
    public Observable<LogLevel> ClientLogLevel { get; set; } = LogLevel.Info;

    [ConfigurationProperty("DEV_PRESENTATION_TRACER_LEVEL", group: CommonConfigurationGroups.LOGGING)]
    public Observable<SourceLevels> PresentationSourceLevel { get; set; } = SourceLevels.Off;
    #endregion
}