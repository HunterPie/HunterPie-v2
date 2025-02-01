using HunterPie.Core.Analytics;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Utils;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Analytics.Entity;
using System;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class ExceptionCatcherInitializer : IInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly IAnalyticsService _analyticsService;

    public ExceptionCatcherInitializer(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public Task Init()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is not Exception exception)
                return;

            AsyncHelper.RunSync(async () =>
            {
                await _analyticsService.SendAsync(
                    analyticsEvent: AnalyticsEvent.FromException(exception, isUiError: false)
                );
            });


            _logger.Error(exception.ToString());
        };

        return Task.CompletedTask;
    }
}