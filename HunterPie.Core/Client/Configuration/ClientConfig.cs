using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Generics;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration;

[Configuration("CLIENT_STRING", "ICON_HUNTERPIE")]
public class ClientConfig : ISettings
{
    #region Auto Update
    [ConfigurationProperty("ENABLE_SELF_UPDATE", group: CommonConfigurationGroups.SELF_UDPATE)]
    public Observable<bool> EnableAutoUpdate { get; set; } = true;

    [ConfigurationProperty("ENABLE_SELF_UPDATE_CONFIRMATION", group: CommonConfigurationGroups.SELF_UDPATE)]
    public Observable<bool> EnableAutoUpdateConfirmation { get; set; } = true;
    #endregion

    #region Supporter
    [ConfigurationProperty("SUPPORTER_SECRET_TOKEN_STRING", group: CommonConfigurationGroups.SUPPORTER)]
    public Secret SupporterSecretToken { get; set; } = new();
    #endregion

    #region General Settings
    [ConfigurationProperty("LANGUAGE_STRING", requiresRestart: true, group: CommonConfigurationGroups.GENERAL)]
    public GenericFileSelector Language { get; set; } = new GenericFileSelector("en-us.xml", "*.xml", ClientInfo.LanguagesPath);

    [ConfigurationProperty("THEME_STRING", requiresRestart: true, group: CommonConfigurationGroups.GENERAL)]
    public GenericFolderSelector Theme { get; set; } = new GenericFolderSelector("Default", "*", ClientInfo.ThemesPath);
    #endregion

    #region Customization Settings
    [ConfigurationProperty("MINIMIZE_TO_SYSTEM_TRAY_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<bool> MinimizeToSystemTray { get; set; } = true;

    [ConfigurationProperty("SEAMLESS_STARTUP_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<bool> EnableSeamlessStartup { get; set; } = false;

    [ConfigurationProperty("SEAMLESS_SHUTDOWN_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<bool> EnableSeamlessShutdown { get; set; } = false;

    [ConfigurationProperty("SHUTDOWN_ON_GAME_EXIT", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<bool> ShouldShutdownOnGameExit { get; set; } = false;
    #endregion

    #region Native Settings
    [ConfigurationProperty("ENABLE_NATIVE_MODULE_STRING", availableGames: GameProcessType.MonsterHunterWorld, group: CommonConfigurationGroups.NATIVE)]
    public Observable<bool> EnableNativeModule { get; set; } = true;
    #endregion

    #region Rendering Settings
    [ConfigurationProperty("RENDERING_STRATEGY_STRING", requiresRestart: true, group: CommonConfigurationGroups.RENDERING)]
    public Observable<RenderingStrategy> Render { get; set; } = RenderingStrategy.Software;

    [ConfigurationProperty("RENDERING_FPS_STRING", requiresRestart: true, group: CommonConfigurationGroups.RENDERING)]
    public Range RenderFramePerSecond { get; set; } = new Range(60, 60, 1, 1);
    #endregion

    #region Scanning Settings
    [ConfigurationProperty("POLLING_RATE_STRING", group: CommonConfigurationGroups.SCANNING)]
    public Range PollingRate { get; set; } = new(50, 1000, 1, 1);
    #endregion

    #region Development Settings
    [ConfigurationProperty("DEV_ENABLE_FEATURE_FLAG", requiresRestart: true, group: CommonConfigurationGroups.DEVELOPMENT)]
    public Observable<bool> EnableFeatureFlags { get; set; } = false;
    #endregion

    public Observable<GameType> DefaultGameType { get; set; } = GameType.Rise;

    // States
    public Observable<GameProcessType> LastConfiguredGame { get; set; } = GameProcessType.MonsterHunterRise;
}