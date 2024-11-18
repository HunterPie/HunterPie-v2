﻿using HunterPie.UI.Architecture;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Home;

internal class HomeSideBarViewModel : ViewModel, ISideBarViewModel
{
    private readonly HomeProvider _provider;

    public Type Type => typeof(HomeViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='HOME_STRING']";

    public string Icon => "ICON_HOME";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public HomeSideBarViewModel(HomeProvider provider)
    {
        _provider = provider;

        _provider.Subscribe();
    }

    public void Execute()
    {
        ObservableCollection<SupportedGameViewModel> supportedGames = _provider.GetSupportedGameViewModels();

        Navigator.Body.Navigate(new HomeViewModel(supportedGames));
    }
}