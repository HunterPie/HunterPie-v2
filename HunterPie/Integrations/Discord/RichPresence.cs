using DiscordRPC;
using DiscordRPC.Message;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Core.Logger;
using System;
using System.ComponentModel;
using System.Timers;
using DiscordPresence = DiscordRPC.RichPresence;

namespace HunterPie.Integrations.Discord;

internal abstract class RichPresence : IDisposable
{
    private const int DEFAULT_INTERVAL = 10000;

    protected abstract DiscordRichPresence Settings { get; }
    protected readonly DiscordPresence Presence = new();

    private readonly IGame _game;
    private readonly DiscordRpcClient _client;
    private readonly Timer _timer = new(DEFAULT_INTERVAL) { AutoReset = true };

    private Timestamps _locationTime = Timestamps.Now;

    protected RichPresence(
        string appId,
        IGame game
    )
    {
        _game = game;
        _client = new(appId, autoEvents: true);

        _ = _client.Initialize();
        _timer.Start();
        HookEvents();
    }

    protected abstract void HandlePresence();

    private void UpdatePresence()
    {
        if (!Settings.EnableRichPresence
            || !_client.IsInitialized)
        {
            _client.ClearPresence();
            return;
        }

        try
        {
            HandlePresence();
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }

        _ = Presence.WithTimestamps(_locationTime);
        _client.SetPresence(Presence);
    }

    private void HookEvents()
    {
        _client.OnReady += OnReady;
        _game.Player.OnStageUpdate += OnStageUpdate;
        _timer.Elapsed += OnTick;
        Settings.EnableRichPresence.PropertyChanged += OnEnableRichPresenceChanged;
    }

    private void UnhookEvents()
    {
        _client.OnReady -= OnReady;
        _game.Player.OnStageUpdate -= OnStageUpdate;
        _timer.Elapsed -= OnTick;
        Settings.EnableRichPresence.PropertyChanged -= OnEnableRichPresenceChanged;
    }

    private void OnReady(object sender, ReadyMessage args)
    {
        if (Settings.EnableRichPresence)
            Log.Info($"Connected to Discord: {args.User}");

        UpdatePresence();
    }

    private void OnTick(object? sender, ElapsedEventArgs e) => UpdatePresence();

    private void OnStageUpdate(object? sender, EventArgs e)
    {
        _locationTime = Timestamps.Now;
        UpdatePresence();
    }

    private void OnEnableRichPresenceChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (Settings.EnableRichPresence)
            UpdatePresence();
        else
            _client.ClearPresence();
    }

    public void Dispose()
    {
        UnhookEvents();
        _client.ClearPresence();
        _client.Dispose();
        _timer.Stop();
        _timer.Dispose();
    }
}