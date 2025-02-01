using HunterPie.Core.Analytics.Entity;
using HunterPie.Features.Analytics.Entity;
using HunterPie.Integrations.Poogie.Report;
using HunterPie.Integrations.Poogie.Report.Models;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Analytics.Strategies;

internal class CrashEventStrategy : IAnalyticsStrategy
{
    private readonly PoogieReportConnector _connector;

    public CrashEventStrategy(PoogieReportConnector connector)
    {
        _connector = connector;
    }

    public bool CanHandle(Type type) => type == typeof(CrashPayload);

    public async Task SendAsync(IAnalyticsEvent analyticsEvent)
    {
        if (analyticsEvent is not CrashPayload payload)
            return;

        var request = new CrashReportRequest(
            Version: payload.Version,
            GameBuild: payload.GameBuild ?? "Unknown",
            Exception: payload.Error,
            Stacktrace: payload.Stacktrace,
            IsUiError: payload.IsUiError,
            Context: new CrashReportContextRequest(
                RamTotal: payload.Context.TotalSystemMemory,
                RamUsed: payload.Context.AllocatedMemory,
                WindowsVersion: payload.Context.WindowsVersion
            )
        );

        await _connector.CrashAsync(request);
    }
}