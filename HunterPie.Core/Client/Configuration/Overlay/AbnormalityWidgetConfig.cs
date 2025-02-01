using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Converters;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;
using Newtonsoft.Json;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration(name: "ABNORMALITY_WIDGET",
    icon: "ICON_STOPWATCH",
    group: CommonConfigurationGroups.OVERLAY)]
public class AbnormalityWidgetConfig : IWidgetSettings, ISettings
{
    [JsonConverter(typeof(ObservableHashSetConverter<string>))]
    public ObservableHashSet<string> AllowedAbnormalities { get; set; } = new();

    #region General Settings
    [ConfigurationProperty("ABNORMALITY_TRAY_NAME_STRING", group: CommonConfigurationGroups.GENERAL)]
    public Observable<string> Name { get; set; } = "Abnormality Tray";

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
    public Position Position { get; set; } = new(100, 100);
    #endregion

    #region Customization Settings
    [ConfigurationProperty("ABNORMALITY_TRAY_SORT_BY_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<SortBy> SortByAlgorithm { get; set; } = SortBy.Off;

    [ConfigurationProperty("ABNORMALITY_TRAY_MAX_SIZE_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Range MaxSize { get; set; } = new(300, 1200, 30, 30);

    [ConfigurationProperty("ORIENTATION_STRING", group: CommonConfigurationGroups.CUSTOMIZATIONS)]
    public Observable<Orientation> Orientation { get; set; } = Enums.Orientation.Horizontal;
    #endregion


}