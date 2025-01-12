using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay;

[Configuration("ABNORMALITY_TRAY_STRING", "ICON_SELFIMPROVEMENT+")]
public class AbnormalityTrayConfig : ISettings
{
    [ConfigurationProperty("ABNORMALITY_TRAYS_STRING", requiresRestart: true, group: CommonConfigurationGroups.ABNORMALITY_TRAYS)]
    public AbnormalityTrays Trays { get; set; } = new();
}