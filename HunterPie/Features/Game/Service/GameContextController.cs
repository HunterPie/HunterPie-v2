using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process.Events;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using HunterPie.Features.Backup.Services;
using HunterPie.Features.Overlay;
using HunterPie.Integrations.Services;
using HunterPie.UI.Overlay;
using System;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.Features.Game.Service;

internal class GameContextController : IDisposable
{
    private bool _isDisposed;
    private Context? _context;
    private readonly IProcessWatcherService _processWatcherService;
    private readonly IGameContextService _gameContextService;
    private readonly IBackupService _backupService;
    private readonly Dispatcher _uiDispatcher;

    public GameContextController(
        IProcessWatcherService processWatcherService,
        IGameContextService gameContextService,
        IBackupService backupService,
        Dispatcher uiDispatcher)
    {
        _processWatcherService = processWatcherService;
        _gameContextService = gameContextService;
        _backupService = backupService;
        _uiDispatcher = uiDispatcher;
        Subscribe();
    }

    public void Subscribe()
    {
        _processWatcherService.ProcessStart += OnProcessStart;
        _processWatcherService.ProcessExit += OnProcessExit;
    }

    private async void OnProcessExit(object? sender, EventArgs e)
    {
        // TODO: Dispose rich presence
        ScanManager.Stop();

        _context?.Dispose();

        _context = null;

        await _uiDispatcher.InvokeAsync(WidgetInitializers.Unload);
        WidgetManager.Dispose();

        Log.Info("Process has closed");

        SmartEventsTracker.DisposeEvents();
        ContextInitializers.Dispose();

        if (ClientConfig.Config.Client.ShouldShutdownOnGameExit)
            _uiDispatcher.Invoke(Application.Current.Shutdown);
    }

    private async void OnProcessStart(object? sender, ProcessEventArgs e)
    {
        // TODO: Create rich presence
        try
        {
            _context = _gameContextService.Get(e.Game);

            Log.Debug("Initialized game context");

            await _uiDispatcher.InvokeAsync(() => WidgetManager.Hook(_context));

            await ContextInitializers.InitializeAsync(_context);

            await _uiDispatcher.Invoke(async () => await WidgetInitializers.InitializeAsync(_context));

            ScanManager.Start();
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }

        await _backupService.ExecuteAsync(e.Game.Type switch
        {
            GameProcessType.MonsterHunterRise => GameType.Rise,
            GameProcessType.MonsterHunterWorld => GameType.World,
            _ => throw new ArgumentOutOfRangeException()
        });
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _processWatcherService.ProcessStart += OnProcessStart;
        _processWatcherService.ProcessExit += OnProcessExit;
        _isDisposed = true;
    }
}