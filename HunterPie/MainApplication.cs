using HunterPie.Core.Analytics;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Game.Service;
using HunterPie.Internal;
using HunterPie.UI.Main.Navigators;
using HunterPie.Update.UseCase;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie;

internal class MainApplication
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IUpdateUseCase _updateUseCase;
    private readonly IRemoteAccountConfigUseCase _remoteAccountConfigUseCase;
    private readonly RemoteConfigSyncService _remoteConfigSyncService;
    private readonly NavigatorController _navigatorController;
    private readonly GameContextController _gameContextController;


    public MainApplication(
        IAnalyticsService analyticsService,
        IUpdateUseCase updateUseCase,
        IRemoteAccountConfigUseCase remoteAccountConfigUseCase,
        RemoteConfigSyncService remoteConfigSyncService,
        NavigatorController navigatorController,
        GameContextController gameContextController)
    {
        _analyticsService = analyticsService;
        _updateUseCase = updateUseCase;
        _remoteAccountConfigUseCase = remoteAccountConfigUseCase;
        _remoteConfigSyncService = remoteConfigSyncService;
        _navigatorController = navigatorController;
        _gameContextController = gameContextController;
    }

    public async Task Start()
    {
#if RELEASE
        await SelfUpdate();
#endif
        _gameContextController.Subscribe();
        _remoteConfigSyncService.Start();
        await _navigatorController.SetupAsync();
    }

    public async Task SendUiException(Exception exception)
    {
        await _analyticsService.SendAsync(
            analyticsEvent: AnalyticsEvent.FromException(exception, isUiError: true)
        );
    }

    public async Task Restart()
    {
        await _remoteAccountConfigUseCase.UploadAsync();

        string executablePath = typeof(MainApplication).Assembly.Location.Replace(".dll", ".exe");
        Process.Start(executablePath);
    }

    private async Task SelfUpdate()
    {
        bool hasUpdated = await _updateUseCase.InvokeAsync();

        if (!hasUpdated)
            return;

        InitializerManager.Unload();
        await Restart();
    }
}