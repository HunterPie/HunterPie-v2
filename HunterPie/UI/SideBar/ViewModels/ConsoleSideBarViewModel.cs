using HunterPie.UI.Architecture;
using HunterPie.UI.Logging.ViewModels;
using HunterPie.UI.Navigation;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class ConsoleSideBarViewModel : ViewModel, ISideBarViewModel
{
    private readonly IBodyNavigator _bodyNavigator;
    private readonly ConsoleViewModel _consoleViewModel;

    public Type Type => typeof(ConsoleViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='CONSOLE_STRING']";

    public string Icon => "ICON_CONSOLE";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public ConsoleSideBarViewModel(
        IBodyNavigator bodyNavigator,
        ConsoleViewModel consoleViewModel)
    {
        _bodyNavigator = bodyNavigator;
        _consoleViewModel = consoleViewModel;
    }

    public Task ExecuteAsync()
    {
        _bodyNavigator.Navigate(_consoleViewModel);

        return Task.CompletedTask;
    }
}