using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Extensions;
using HunterPie.Features.Settings.Factory;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.Internal;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.UI.Home.Services;

internal class HomeService
{
    private readonly IGameWatcher[] _gameWatchers;
    private readonly IBodyNavigator _bodyNavigator;
    private readonly SettingsFactory _settingsFactory;

    private static readonly Dictionary<GameType, SupportedGameViewModel> SupportedGames = new()
    {
        {
            GameType.World, new SupportedGameViewModel
            {
                Banner = "https://cdn.hunterpie.com/resources/monster-hunter-world-poster.jpg",
                Execute = new Action(static () => Steam.RunGameBy(type: GameType.World)),
                Name = Games.MONSTER_HUNTER_WORLD
            }
        },
        {
            GameType.Rise, new SupportedGameViewModel
            {
                Banner =
                    "https://cdn.hunterpie.com/resources/monster-hunter-rise-poster.jpg",
                Execute = new Action(static () => Steam.RunGameBy(type: GameType.Rise)),
                Name = Games.MONSTER_HUNTER_RISE
            }
        }
    };

    public HomeService(
        IGameWatcher[] gameWatchers,
        IBodyNavigator bodyNavigator,
        SettingsFactory settingsFactory)
    {
        _gameWatchers = gameWatchers;
        _bodyNavigator = bodyNavigator;
        _settingsFactory = settingsFactory;
    }

    public void Subscribe()
    {
        foreach (IGameWatcher watcher in _gameWatchers)
        {
            GameType? possibleGameType = MapFactory.Map<GameProcessType, GameType?>(watcher.Game);

            if (possibleGameType is not { } gameType)
                continue;

            SupportedGameViewModel? supportedGame = SupportedGames.GetValueOrDefault(gameType);

            if (supportedGame is not { })
                continue;

            supportedGame.IsAvailable = watcher.Status == ProcessStatus.Waiting;
            supportedGame.Status = watcher.Status;
            supportedGame.OnSettings = BuildSettingsHandler(watcher.Game);

            watcher.StatusChange += (_, args) =>
            {
                supportedGame.IsAvailable = args.NewValue == ProcessStatus.Waiting;
                supportedGame.Status = args.NewValue;
            };
        }
    }

    public ObservableCollection<SupportedGameViewModel> GetSupportedGameViewModels() =>
        SupportedGames.Values.ToObservableCollection();

    private Func<Task> BuildSettingsHandler(GameProcessType gameProcessType)
    {
        return async () =>
        {
            SettingsViewModel viewModel = await _settingsFactory.CreatePartialAsync(gameProcessType);
            _bodyNavigator.Navigate(viewModel);
        };
    }
}