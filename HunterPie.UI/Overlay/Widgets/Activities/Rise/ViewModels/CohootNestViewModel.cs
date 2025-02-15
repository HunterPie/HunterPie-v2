using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class CohootNestViewModel : ViewModel
{
    private string _name;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    public ObservableCollection<CohootItemViewModel> Items { get; } = new();

    public void SetMaxItems(int count)
    {
        if (Items.Count == count)
            return;

        for (int i = 0; i < count; i++)
            Items.Add(
                new CohootItemViewModel
                {
                    IsActive = false
                });
    }

    public void SetItems(int count)
    {
        for (int i = 0; i < Items.Count; i++)
            Items[i].IsActive = i < count;
    }
}