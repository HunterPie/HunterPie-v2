using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration(name: "ABNORMALITY_TRAY_STRING",
    icon: "ICON_SELFIMPROVEMENT+",
    group: CommonConfigurationGroups.OVERLAY)]
public class AbnormalityTrayConfig : ISettings
{
    [ConfigurationProperty("ABNORMALITY_TRAYS_STRING", requiresRestart: true, group: CommonConfigurationGroups.ABNORMALITY_TRAYS)]
    public AbnormalityTrays Trays { get; set; } = new();
}