using HunterPie.Core.Crypto;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics;
using HunterPie.Integrations.Poogie.Statistics.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace HunterPie.Features.Statistics.Services;

internal class QuestTrackerService : IContextInitializer, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly PoogieStatisticsConnector _connector;
    private readonly IAccountUseCase _accountUseCase;
    private readonly AccountConfig _accountConfig;

    private IContext? _context;
    private HuntStatisticsService? _statisticsService;

    public QuestTrackerService(
        PoogieStatisticsConnector connector,
        IAccountUseCase accountUseCase,
        AccountConfig accountConfig)
    {
        _connector = connector;
        _accountUseCase = accountUseCase;
        _accountConfig = accountConfig;
    }

    private void HookEvents()
    {
        if (_context is null)
            return;

        _context.Game.OnQuestStart += OnQuestStart;
        _context.Game.OnQuestEnd += OnQuestEnd;
    }

    private void UnhookEvents()
    {
        if (_context is null)
            return;

        _context.Game.OnQuestStart -= OnQuestStart;
        _context.Game.OnQuestEnd -= OnQuestEnd;
    }

    private async void OnQuestEnd(object? sender, QuestEndEventArgs e)
    {
        _logger.Debug($"Quest ended with status {e.Status}");

        if (_statisticsService is null)
            return;

        if (!await _accountUseCase.IsValidSessionAsync() || !_accountConfig.IsHuntUploadEnabled)
            return;

        HuntStatisticsModel exported = _statisticsService.Export();

        if (e.Status != QuestStatus.Success || !ShouldUpload(exported))
        {
            _logger.Debug($"Quest not uploaded (status: {e.Status}, monsters: {exported.Monsters.Count})");
            return;
        }

        DateTime questFinishedAt = exported.StartedAt.Add(e.TimeElapsed);
        string newHash = await GenerateUniqueHashAsync(questFinishedAt, exported.Hash);

        exported = exported with
        {
            FinishedAt = questFinishedAt,
            Hash = newHash
        };

        var exportedRequest = PoogieQuestStatisticsModel.From(exported);

        PoogieResult<PoogieQuestStatisticsModel> result = await _connector.UploadAsync(exportedRequest)
            .ConfigureAwait(false);

        if (result.Error is not { })
            _logger.Debug("Quest uploaded successfully");

        _statisticsService.Dispose();
    }

    private void OnQuestStart(object? sender, IQuest e)
    {
        if (_context is null)
            return;

        bool shouldIgnore = e.Type switch
        {
            QuestType.Hunt
                or QuestType.Slay
                or QuestType.Capture => false,
            _ => true
        };

        if (shouldIgnore)
            return;

        _logger.Debug($"Quest started (id: {e.Id}, type: {e.Type})");

        _statisticsService?.Dispose();
        _statisticsService = new HuntStatisticsService(_context);
    }

    public void Dispose()
    {
        UnhookEvents();
        _statisticsService?.Dispose();
    }

    private static async Task<string> GenerateUniqueHashAsync(DateTime questFinishedAt, string currentHash)
    {
        string questTimeFormatted = questFinishedAt.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
        return await HashService.HashAsync($"{currentHash}:{questTimeFormatted}");
    }

    private static bool ShouldUpload(HuntStatisticsModel model)
    {
        return model.Monsters.Count > 0;
    }

    public Task InitializeAsync(IContext context)
    {
        _context = context;
        HookEvents();

        return Task.CompletedTask;
    }
}