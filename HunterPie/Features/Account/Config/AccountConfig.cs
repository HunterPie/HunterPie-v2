using HunterPie.Core.Architecture;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.Core.Settings.Common;

namespace HunterPie.Features.Account.Config;

[Configuration(name: "ACCOUNT_STRING",
    icon: "ICON_ACCOUNT",
    group: CommonConfigurationGroups.ACCOUNT)]
internal class AccountConfig : ISettings
{
    #region General settings
    [ConfigurationProperty("ACCOUNT_ENABLE_BACKUP_STRING", group: CommonConfigurationGroups.GENERAL)]
    public Observable<bool> IsBackupEnabled { get; set; } = true;

    [ConfigurationProperty("ACCOUNT_ENABLE_HUNT_UPLOAD_STRING", group: CommonConfigurationGroups.GENERAL)]
    public Observable<bool> IsHuntUploadEnabled { get; set; } = true;
    #endregion
}