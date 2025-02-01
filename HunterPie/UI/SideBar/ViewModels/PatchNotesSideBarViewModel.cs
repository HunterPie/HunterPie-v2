using HunterPie.DI;
using HunterPie.Features.Patches.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class PatchNotesSideBarViewModel : ViewModel, ISideBarViewModel
{
    private readonly IBodyNavigator _bodyNavigator;

    public Type Type => typeof(PatchesViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='PATCH_NOTES_STRING']";

    public string Icon => "ICON_DOCUMENTATION";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public PatchNotesSideBarViewModel(IBodyNavigator bodyNavigator)
    {
        _bodyNavigator = bodyNavigator;
    }

    public Task ExecuteAsync()
    {
        PatchesViewModel viewModel = DependencyContainer.Get<PatchesViewModel>();

        _bodyNavigator.Navigate(viewModel);

        return Task.CompletedTask;
    }
}