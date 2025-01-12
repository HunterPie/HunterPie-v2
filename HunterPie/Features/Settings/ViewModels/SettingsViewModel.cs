using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Core.Search;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.Integrations.Poogie.Version.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.Features.Settings.ViewModels;

internal class SettingsViewModel : ViewModel
{
    private readonly PoogieVersionConnector _connector;
    private readonly Dictionary<GameProcessType, ObservableCollection<ConfigurationCategory>> _configurations;

    public ObservableCollection<GameProcessType> ConfigurableGames { get; }
    public Observable<GameProcessType> SelectedGameConfiguration { get; }

    private UpdateFetchStatus _updateStatus = UpdateFetchStatus.Fetching;
    public UpdateFetchStatus UpdateStatus { get => _updateStatus; set => SetValue(ref _updateStatus, value); }

    private int _currentTabIndex;
    public int CurrentTabIndex { get => _currentTabIndex; set => SetValue(ref _currentTabIndex, value); }

    private ObservableCollection<ConfigurationCategory> _categories;
    public ObservableCollection<ConfigurationCategory> Categories { get => _categories; set => SetValue(ref _categories, value); }

    private DateTime _synchronizedAt = DateTime.Now;
    public DateTime SynchronizedAt { get => _synchronizedAt; set => SetValue(ref _synchronizedAt, value); }

    public SettingsViewModel(
        Dictionary<GameProcessType, ObservableCollection<ConfigurationCategory>> configurations,
        ObservableCollection<GameProcessType> configurableGames,
        Observable<GameProcessType> currentConfiguredGame,
        PoogieVersionConnector connector)
    {
        _configurations = configurations;
        ConfigurableGames = configurableGames;
        SelectedGameConfiguration = currentConfiguredGame;
        _connector = connector;
        _categories = _configurations[currentConfiguredGame.Value];
    }

    public async void FetchVersion()
    {
        UpdateStatus = UpdateFetchStatus.Fetching;
        PoogieResult<VersionResponse> response = await _connector.Latest();

        switch (response)
        {
            case { Error: { } }:
                UpdateStatus = UpdateFetchStatus.Error;
                return;
            case { Response: { } versionResponse }:
                var version = new Version(versionResponse.LatestVersion);
                UpdateStatus = ClientInfo.IsVersionGreaterOrEq(version)
                    ? UpdateFetchStatus.Latest
                    : UpdateFetchStatus.NeedsUpdate;
                break;
        }
    }

    public void Search(string query)
    {
        Categories.SelectMany(it => it.Groups.SelectMany(group => group.Properties))
            .ForEach(it =>
            {
                if (it is not ConfigurationPropertyViewModel vm)
                    return;

                vm.IsMatch = SearchEngine.IsMatch($"{vm.Name} {vm.Description}", query);
            });
    }

    public void ChangeSettingsGroup()
    {
        ObservableCollection<ConfigurationCategory> newCategories = _configurations[SelectedGameConfiguration];
        if (Categories == newCategories)
            return;

        Categories = newCategories;
        CurrentTabIndex = 0;
    }

    public void ExecuteUpdate() => App.Restart();
}