using HunterPie.UI.Architecture;

namespace HunterPie.UI.Home.ViewModels;

internal class HomeNewsItemViewModel : ViewModel
{
    private string _title = string.Empty;
    public string Title { get => _title; set => SetValue(ref _title, value); }

    private string _description = string.Empty;
    public string Description { get => _description; set => SetValue(ref _description, value); }

    private string? _banner = null;
    public string? Banner { get => _banner; set => SetValue(ref _banner, value); }
}