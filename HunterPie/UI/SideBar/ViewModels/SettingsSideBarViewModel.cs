using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Client.Events;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Settings.Factory;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class SettingsSideBarViewModel : ViewModel, ISideBarViewModel
{
    private readonly IBodyNavigator _bodyNavigator;
    private readonly LocalAccountConfig _localAccountConfig;
    private readonly SettingsFactory _settingsFactory;

    private SettingsViewModel? _viewModel;
    public Type Type => typeof(SettingsViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='SETTINGS_STRING']";

    public string Icon => "ICON_SETTINGS";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public SettingsSideBarViewModel(
        IBodyNavigator bodyNavigator,
        LocalAccountConfig localAccountConfig,
        SettingsFactory settingsFactory)
    {
        _bodyNavigator = bodyNavigator;
        _localAccountConfig = localAccountConfig;
        _settingsFactory = settingsFactory;

        ConfigManager.OnSync += OnConfigurationSync;
    }

    private void OnConfigurationSync(object? sender, ConfigSaveEventArgs e)
    {
        if (Path.GetFileNameWithoutExtension(e.Path) != "config")
            return;

        if (_viewModel is not { })
            return;

        _viewModel.SynchronizedAt = e.SyncedAt;
    }

    public async Task ExecuteAsync()
    {
        SettingsViewModel viewModel = await BuildViewModelAsync();

        _bodyNavigator.Navigate(viewModel);
    }

    private async Task<SettingsViewModel> BuildViewModelAsync()
    {
        if (_viewModel is { })
            return _viewModel;

        _viewModel = await _settingsFactory.CreateFullAsync(currentGame: ClientConfig.Config.Client.LastConfiguredGame);

        return _viewModel;
    }

    private static ObservableCollection<ConfigurationCategory> BuildConfiguration(
        IEnumerable<ConfigurationCategory> commonConfiguration,
        GameConfig configuration,
        GameProcessType gameProcessType
    )
    {
        ObservableCollection<ConfigurationCategory> configCategory = ConfigurationAdapter.Adapt(configuration, gameProcessType);

        return commonConfiguration.Concat(configCategory)
            .ToObservableCollection();
    }
}