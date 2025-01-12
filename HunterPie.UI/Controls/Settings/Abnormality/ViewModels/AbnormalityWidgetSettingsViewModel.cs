using HunterPie.Core.Architecture;
using HunterPie.Core.Extensions;
using HunterPie.Core.Search;
using HunterPie.UI.Navigation;
using HunterPie.UI.Settings.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Controls.Settings.Abnormality.ViewModels;

#nullable enable
public class AbnormalityWidgetSettingsViewModel : Architecture.ViewModel
{
    public ObservableCollection<AbnormalityCategoryViewModel> Categories { get; }
    public ObservableHashSet<string> SelectedAbnormalities { get; }
    public ConfigurationCategory Configuration { get; }

    private AbnormalityCategoryViewModel _selectedCategory;
    public AbnormalityCategoryViewModel SelectedCategory { get => _selectedCategory; set => SetValue(ref _selectedCategory, value); }

    public AbnormalityWidgetSettingsViewModel(
        ConfigurationCategory configuration,
        ObservableCollection<AbnormalityCategoryViewModel> categories,
        ObservableHashSet<string> selectedAbnormalities
    )
    {
        _selectedCategory = categories.First();
        Configuration = configuration;
        Categories = categories;
        SelectedAbnormalities = selectedAbnormalities;
    }

    public void SelectAllFromCurrentCategory()
    {
        IEnumerable<string> selectedElements = SelectedCategory.Elements.Select(it => it.Id);
        SelectedAbnormalities.UnionWith(selectedElements);
    }

    public void UnselectAllFromCurrentCategory()
    {
        IEnumerable<string> removedElements = SelectedCategory.Elements.Select(it => it.Id);
        SelectedAbnormalities.ExceptWith(removedElements);
    }

    public void Search(string query)
    {
        Categories.ForEach(it =>
            it.Elements.AsParallel()
                .ForEach(vm =>
                    vm.IsMatch = string.IsNullOrEmpty(query) || SearchEngine.IsMatch(vm.Name, query)
                )
        );
    }

    public void ToggleAbnormality(string abnormalityId)
    {
        if (SelectedAbnormalities.Contains(abnormalityId))
            SelectedAbnormalities.Remove(abnormalityId);
        else
            SelectedAbnormalities.Add(abnormalityId);
    }

    public void ExitScreen()
    {
        Navigator.Body.Return();
    }
}