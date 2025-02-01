using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Backup;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Localization;
using HunterPie.Integrations.Poogie.Notification;
using HunterPie.Integrations.Poogie.Patch;
using HunterPie.Integrations.Poogie.Report;
using HunterPie.Integrations.Poogie.Settings;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Supporter;
using HunterPie.Integrations.Poogie.Version;

namespace HunterPie.Integrations.Poogie;

internal class PoogieModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithService<PoogieConnector>()
            .WithSingle<PoogieHttpProvider>()
            .WithService<PoogieAccountConnector>()
            .WithService<PoogieBackupConnector>()
            .WithService<PoogieLocalizationConnector>()
            .WithService<PoogieNotificationConnector>()
            .WithService<PoogiePatchConnector>()
            .WithService<PoogieReportConnector>()
            .WithService<PoogieClientSettingsConnector>()
            .WithSingle<PoogieStatisticsConnector>()
            .WithService<PoogieSupporterConnector>()
            .WithService<PoogieVersionConnector>();
    }
}