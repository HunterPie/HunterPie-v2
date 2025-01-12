using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Adapter;

namespace HunterPie.Features.Statistics.ViewModels;
public class MonsterSummaryViewModel : ViewModel
{
    private GameType _gameType;
    public GameType GameType
    {
        get => _gameType;
        set => SetValue(ref _gameType, value);
    }

    private int _id;
    public int Id
    {
        get => _id;
        set => SetValue(ref _id, value);
    }

    private string? _icon;
    public string? Icon
    {
        get => _icon;
        set => SetValue(ref _icon, value);
    }

    private bool _isTarget;
    public bool IsTarget
    {
        get => _isTarget;
        set => SetValue(ref _isTarget, value);
    }

    private MonsterHuntType? _huntType;
    public MonsterHuntType? HuntType { get => _huntType; set => SetValue(ref _huntType, value); }

    public MonsterSummaryViewModel() { }

    internal MonsterSummaryViewModel(
        GameType game,
        PoogieMonsterSummaryModel model)
    {
        GameType = game;
        Id = model.Id;
        IsTarget = model.IsTarget;
        HuntType = model.HuntType;

        FetchMonsterIcon();
    }

    private async void FetchMonsterIcon()
    {
        Icon = await MonsterIconAdapter.UriFrom(GameType, Id);
    }
}