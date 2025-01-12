using HunterPie.Core.Client;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Header.ViewModels;

internal class HeaderViewModel : ViewModel
{
    private string _version = $"v{ClientInfo.Version}";
    public string Version { get => _version; set => SetValue(ref _version, value); }

    private bool _isAdmin = ClientInfo.IsAdmin;
    public bool IsAdmin { get => _isAdmin; set => SetValue(ref _isAdmin, value); }

    public AccountMenuViewModel AccountMenuViewModel { get; }

    public HeaderViewModel(AccountMenuViewModel accountMenuViewModel)
    {
        AccountMenuViewModel = accountMenuViewModel;
    }
}