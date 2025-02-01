using HunterPie.UI.Architecture;

namespace HunterPie.Features.Settings.ViewModels;

internal class ConfigurationCategoryTitle : ViewModel, IConfigurationCategory
{
    private string _title = string.Empty;
    public string Title { get => _title; set => SetValue(ref _title, value); }
}