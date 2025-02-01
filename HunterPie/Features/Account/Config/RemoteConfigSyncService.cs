using HunterPie.Core.Client;
using HunterPie.Core.Client.Events;
using HunterPie.Core.Extensions;
using HunterPie.Features.Account.UseCase;
using System;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace HunterPie.Features.Account.Config;
internal class RemoteConfigSyncService
{
    private readonly IRemoteAccountConfigUseCase _remoteAccountConfigService;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly Timer _timer = new(1000) { AutoReset = true };

    private DateTime _lastSync = DateTime.Now;
    private DateTime _lastUpload = DateTime.Now;

    public RemoteConfigSyncService(IRemoteAccountConfigUseCase remoteAccountConfigService)
    {
        _remoteAccountConfigService = remoteAccountConfigService;
    }

    public void Start()
    {
        ConfigManager.OnSync += OnConfigSync;
        _timer.Elapsed += OnTimerTick;
        _timer.Start();
    }

    private async void OnTimerTick(object? sender, ElapsedEventArgs e)
    {
        try
        {
            await _semaphore.WaitAsync();

            if (!_lastSync.HasElapsed(_lastUpload, TimeSpan.FromSeconds(5)))
                return;

            _lastUpload = _lastSync;

            await _remoteAccountConfigService.Upload();
        }
        catch { }
        finally
        {
            _semaphore.Release();
        }
    }

    private async void OnConfigSync(object? sender, ConfigSaveEventArgs e)
    {
        try
        {
            await _semaphore.WaitAsync();

            _lastSync = e.SyncedAt;
        }
        catch { }
        finally
        {
            _semaphore.Release();
        }
    }
}