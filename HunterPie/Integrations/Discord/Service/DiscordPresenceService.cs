using DiscordRPC;
using DiscordRPC.Message;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Integrations.Discord.Strategies;
using System;
using System.ComponentModel;
using System.Timers;

namespace HunterPie.Integrations.Discord.Service;

internal class DiscordPresenceService : IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private const int UPDATE_INTERVAL_MS = 10_000;
    private readonly RichPresence _presence = new();
    private Timestamps _stageElapsedTime = Timestamps.Now;

    private readonly Timer _timer = new(UPDATE_INTERVAL_MS) { AutoReset = true };
    private readonly IContext _context;
    private readonly DiscordRpcClient _client;
    private readonly DiscordRichPresence _configuration;
    private readonly IDiscordRichPresenceStrategy _strategy;

    public DiscordPresenceService(
        IContext context,
        DiscordRichPresence configuration,
        IDiscordRichPresenceStrategy strategy)
    {
        _context = context;
        _client = new DiscordRpcClient(
            applicationID: strategy.AppId,
            autoEvents: true
        );
        _configuration = configuration;
        _strategy = strategy;
    }

    public void Start()
    {
        _client.OnReady += OnDiscordClientReady;
        _context.Game.Player.OnStageUpdate += OnPlayerStageUpdate;
        _timer.Elapsed += OnTimerTick;
        _configuration.EnableRichPresence.PropertyChanged += OnEnableRichPresenceChanged;

        _client.Initialize();
        _timer.Start();
    }

    private void OnEnableRichPresenceChanged(object? sender, PropertyChangedEventArgs e) =>
        HandlePresenceUpdate();

    private void OnTimerTick(object? sender, ElapsedEventArgs e) =>
        HandlePresenceUpdate();

    private void OnPlayerStageUpdate(object? sender, EventArgs e)
    {
        _stageElapsedTime = Timestamps.Now;
        HandlePresenceUpdate();
    }

    private void OnDiscordClientReady(object sender, ReadyMessage args)
    {
        if (!_configuration.EnableRichPresence)
            return;

        _logger.Info($"Connected to Discord Rich Presence: {args.User}");
        HandlePresenceUpdate();
    }

    private void HandlePresenceUpdate()
    {
        if (!_configuration.EnableRichPresence
            || !_client.IsInitialized)
        {
            _client.ClearPresence();
            return;
        }

        try
        {
            _strategy.Update(_presence);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.ToString());
        }

        _presence.WithTimestamps(_stageElapsedTime);

        _client.SetPresence(_presence);
    }

    public void Dispose()
    {
        _configuration.EnableRichPresence.PropertyChanged -= OnEnableRichPresenceChanged;
        _client.OnReady -= OnDiscordClientReady;
        _context.Game.Player.OnStageUpdate -= OnPlayerStageUpdate;
        _timer.Elapsed -= OnTimerTick;
        _timer.Dispose();
        _client.Dispose();
    }
}