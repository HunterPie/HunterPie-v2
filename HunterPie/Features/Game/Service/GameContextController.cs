using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process.Events;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Features.Backup.Services;
using HunterPie.Features.Overlay;
using HunterPie.Features.Scan.Service;
using HunterPie.Integrations.Discord.Factory;
using HunterPie.Integrations.Discord.Service;
using HunterPie.Integrations.Services;
using HunterPie.UI.Overlay;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.Features.Game.Service;

internal class GameContextController : IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private bool _isDisposed;
    private Context? _context;
    private readonly Dispatcher _uiDispatcher;
    private readonly IProcessWatcherService _processWatcherService;
    private readonly IGameContextService _gameContextService;
    private readonly IBackupService _backupService;
    private readonly IControllableScanService _controllableScanService;
    private readonly DiscordPresenceFactory _discordPresenceFactory;

    private CancellationTokenSource? _cancellationTokenSource;
    private DiscordPresenceService? _discordPresenceService;

    public GameContextController(
        Dispatcher uiDispatcher,
        IProcessWatcherService processWatcherService,
        IGameContextService gameContextService,
        IBackupService backupService,
        IControllableScanService controllableScanService,
        DiscordPresenceFactory discordPresenceFactory)
    {
        _uiDispatcher = uiDispatcher;
        _processWatcherService = processWatcherService;
        _gameContextService = gameContextService;
        _backupService = backupService;
        _controllableScanService = controllableScanService;
        _discordPresenceFactory = discordPresenceFactory;
    }

    public void Subscribe()
    {
        _processWatcherService.ProcessStart += OnProcessStart;
        _processWatcherService.ProcessExit += OnProcessExit;
    }

    private async void OnProcessStart(object? sender, ProcessEventArgs e)
    {
        try
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _context = _gameContextService.Get(e.Game);

            _logger.Debug("Initialized game context");

            _discordPresenceService = _discordPresenceFactory.Create(_context);
            _discordPresenceService.Start();

            await _uiDispatcher.InvokeAsync(() => WidgetManager.Hook(_context));

            await ContextInitializers.InitializeAsync(_context);

            await _uiDispatcher.InvokeAsync(() => WidgetInitializers.InitializeAsync(_context));

            _controllableScanService.Start(_cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }

        await _backupService.ExecuteAsync(e.Game.Type switch
        {
            GameProcessType.MonsterHunterRise => GameType.Rise,
            GameProcessType.MonsterHunterWorld => GameType.World,
            _ => throw new ArgumentOutOfRangeException()
        });
    }

    private async void OnProcessExit(object? sender, EventArgs e)
    {
        if (_cancellationTokenSource is { })
            await _cancellationTokenSource.CancelAsync();

        _discordPresenceService?.Dispose();

        _context?.Dispose();

        _context = null;

        await _uiDispatcher.InvokeAsync(WidgetInitializers.Unload);
        WidgetManager.Dispose();

        _logger.Info("Process has closed");

        SmartEventsTracker.DisposeEvents();
        ContextInitializers.Dispose();

        if (ClientConfig.Config.Client.ShouldShutdownOnGameExit)
            _uiDispatcher.Invoke(Application.Current.Shutdown);
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _processWatcherService.ProcessStart += OnProcessStart;
        _processWatcherService.ProcessExit += OnProcessExit;
        _cancellationTokenSource?.Dispose();
        _isDisposed = true;
    }
}