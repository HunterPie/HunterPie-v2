using HunterPie.Core.Extensions;
using HunterPie.Core.Search;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterConfigurationsViewModel : ViewModel
{
    private readonly MonsterDetailsConfiguration _configuration;
    private readonly MonsterConfigurationViewModel[] _elements;

    private bool _isSearching;
    public bool IsSearching { get => _isSearching; set => SetValue(ref _isSearching, value); }

    public ObservableCollection<MonsterConfigurationViewModel> Overrides { get; }

    public ObservableCollection<MonsterConfigurationViewModel> SearchElements { get; }

    public MonsterGlobalConfigurationViewModel GlobalConfiguration { get; }

    public MonsterConfigurationsViewModel(
        MonsterGlobalConfigurationViewModel globalConfiguration,
        MonsterDetailsConfiguration configuration,
        MonsterConfigurationViewModel[] elements
    )
    {
        _configuration = configuration;
        _elements = elements;
        GlobalConfiguration = globalConfiguration;
        Overrides = elements.Where(it => it.IsOverriding)
            .ToObservableCollection();
        SearchElements = elements.Where(it => !it.IsOverriding)
            .ToObservableCollection();
    }

    public void FetchIcons()
    {
        foreach (MonsterConfigurationViewModel viewModel in _elements)
            viewModel.FetchIcon();
    }

    public void FilterQuery(string query)
    {
        foreach (MonsterConfigurationViewModel viewModel in SearchElements)
            viewModel.IsMatch = string.IsNullOrEmpty(query) || SearchEngine.IsMatch(viewModel.Name, query);
    }

    public void CreateOverride(MonsterConfigurationViewModel viewModel)
    {
        viewModel.IsOverriding = true;
        Overrides.Add(viewModel);
        SearchElements.Remove(viewModel);
        _configuration.Monsters.Add(viewModel.Configuration);
    }

    public void RemoveOverride(MonsterConfigurationViewModel viewModel)
    {
        viewModel.IsOverriding = false;
        Overrides.Remove(viewModel);
        SearchElements.Add(viewModel);
        _configuration.Monsters.Remove(viewModel.Configuration);
    }
}