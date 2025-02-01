using HunterPie.Core.Architecture;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration(
    name: "OVERLAY_STRING",
    icon: "ICON_OVERLAY",
    group: CommonConfigurationGroups.OVERLAY)]
public class OverlayClientConfig : ISettings
{
    #region General Settings
    [ConfigurationProperty("OVERLAY_ENABLED_STRING", group: CommonConfigurationGroups.GENERAL)]
    public Observable<bool> IsEnabled { get; set; } = true;

    [ConfigurationProperty("OVERLAY_HIDE_WHEN_GAME_UNFOCUS_STRING", group: CommonConfigurationGroups.GENERAL)]
    public Observable<bool> HideWhenUnfocus { get; set; } = false;
    #endregion

    #region Hotkeys Settings
    [ConfigurationProperty("OVERLAY_KEYBINDING_TOGGLE_VISIBILITY_STRING", requiresRestart: true, group: CommonConfigurationGroups.HOTKEYS)]
    public Keybinding ToggleVisibility { get; set; } = "Ctrl+Alt+O";

    [ConfigurationProperty("OVERLAY_KEYBINDING_TOGGLE_DESIGN_MODE", requiresRestart: true, group: CommonConfigurationGroups.HOTKEYS)]
    public Keybinding ToggleDesignMode { get; set; } = "ScrollLock";
    #endregion
}