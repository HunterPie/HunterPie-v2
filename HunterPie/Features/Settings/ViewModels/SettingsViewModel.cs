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
    private readonly Dictionary<GameProcessType, ObservableCollection<IConfigurationCategory>> _configurations;

    public ObservableCollection<GameProcessType> ConfigurableGames { get; }
    public Observable<GameProcessType> SelectedGameConfiguration { get; }

    private UpdateFetchStatus _updateStatus = UpdateFetchStatus.Fetching;
    public UpdateFetchStatus UpdateStatus { get => _updateStatus; set => SetValue(ref _updateStatus, value); }

    private int _currentTabIndex;
    public int CurrentTabIndex { get => _currentTabIndex; set => SetValue(ref _currentTabIndex, value); }

    private ObservableCollection<IConfigurationCategory> _categories;
    public ObservableCollection<IConfigurationCategory> Categories { get => _categories; set => SetValue(ref _categories, value); }

    private DateTime _synchronizedAt = DateTime.Now;
    public DateTime SynchronizedAt { get => _synchronizedAt; set => SetValue(ref _synchronizedAt, value); }

    public SettingsViewModel(
        Dictionary<GameProcessType, ObservableCollection<ConfigurationCategoryGroup>> configurations,
        ObservableCollection<GameProcessType> configurableGames,
        Observable<GameProcessType> currentConfiguredGame,
        PoogieVersionConnector connector)
    {
        _configurations = BuildConfigurationViewModels(configurations);
        ConfigurableGames = configurableGames;
        SelectedGameConfiguration = currentConfiguredGame;
        _connector = connector;
        _categories = _configurations[currentConfiguredGame.Value];

        NavigateToFirstTab();
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
        Categories
            .TryCast<ConfigurationCategoryTab>()
            .Select(it => it.Category)
            .SelectMany(it => it.Groups.SelectMany(group => group.Properties))
            .ForEach(it =>
            {
                if (it is not ConfigurationPropertyViewModel vm)
                    return;

                vm.IsMatch = SearchEngine.IsMatch($"{vm.Name} {vm.Description}", query);
            });
    }

    public void ChangeSettingsGroup()
    {
        ObservableCollection<IConfigurationCategory> newCategories = _configurations[SelectedGameConfiguration];

        if (Categories == newCategories)
            return;

        Categories = newCategories;
        NavigateToFirstTab();
    }

    public void ExecuteUpdate() => App.Restart();

    private Dictionary<GameProcessType, ObservableCollection<IConfigurationCategory>> BuildConfigurationViewModels(
        Dictionary<GameProcessType, ObservableCollection<ConfigurationCategoryGroup>> configurations
    )
    {
        return configurations.ToDictionary(
            keySelector: it => it.Key,
            elementSelector: it =>
                it.Value.SelectMany(group =>
                {
                    List<IConfigurationCategory> viewModels = new(group.Categories.Count + 1)
                    {
                        new ConfigurationCategoryTitle { Title = group.Name }
                    };

                    viewModels.AddRange(group.Categories.Select(category => new ConfigurationCategoryTab { Category = category }));

                    return viewModels;
                }).ToObservableCollection()
        );
    }

    private void NavigateToFirstTab()
    {
        IConfigurationCategory? firstTab = Categories.FirstOrDefault(it => it is ConfigurationCategoryTab);

        if (firstTab is null)
            return;

        int index = Categories.IndexOf(firstTab);

        CurrentTabIndex = index;
    }
}