using HunterPie.Core.Architecture.Collections;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using GameType = HunterPie.Core.Client.Configuration.Enums.GameType;

namespace HunterPie.Features.Statistics.ViewModels;
public class QuestStatisticsSummaryViewModel : ViewModel
{
    public string? UploadId { get; }

    private string? _questName;
    public string? QuestName { get => _questName; set => SetValue(ref _questName, value); }

    private QuestLevel? _questLevel;
    public QuestLevel? QuestLevel { get => _questLevel; set => SetValue(ref _questLevel, value); }

    private int? _stars;
    public int? Stars { get => _stars; set => SetValue(ref _stars, value); }

    private string? _questType;
    public string? QuestType { get => _questType; set => SetValue(ref _questType, value); }

    private int _deaths;
    public int Deaths { get => _deaths; set => SetValue(ref _deaths, value); }

    private int _maxDeaths;
    public int MaxDeaths { get => _maxDeaths; set => SetValue(ref _maxDeaths, value); }

    private TimeSpan? _questTime;
    public TimeSpan? QuestTime { get => _questTime; set => SetValue(ref _questTime, value); }

    private GameType _gameType;
    public GameType GameType
    {
        get => _gameType;
        set => SetValue(ref _gameType, value);
    }

    private DateTime _uploadedAt;
    public DateTime UploadedAt
    {
        get => _uploadedAt;
        set => SetValue(ref _uploadedAt, value);
    }

    public ObservableCollectionRange<MonsterSummaryViewModel> Monsters { get; } = new();

    public QuestStatisticsSummaryViewModel() { }

    internal QuestStatisticsSummaryViewModel(PoogieQuestSummaryModel model)
    {
        UploadId = model.Id;
        GameType = model.GameType.ToEntity();
        UploadedAt = model.CreatedAt.ToLocalTime();
        QuestTime = model.ElapsedTime;



        IEnumerable<MonsterSummaryViewModel> monsterVms =
            model.Monsters.Select(it => new MonsterSummaryViewModel(GameType, it));

        Monsters.Replace(monsterVms);

        if (model.QuestDetails is not { } details)
            return;

        QuestName = Localization.GetQuestNameBy(GameType, details.Id);
        Deaths = details.Deaths;
        MaxDeaths = details.MaxDeaths;
        QuestType = Localization.GetEnumString(details.Type);
        QuestLevel = details.Level;
        Stars = details.Stars;
    }
}