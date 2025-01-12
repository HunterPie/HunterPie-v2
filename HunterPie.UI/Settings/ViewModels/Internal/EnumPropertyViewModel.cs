using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.ViewModels.Internal;

#nullable enable
internal class EnumPropertyViewModel : ConfigurationPropertyViewModel
{
    private object _selectedElement;
    public object SelectedElement { get => _selectedElement; set => SetValue(ref _selectedElement, value); }
    public ObservableCollection<object> Values { get; }

    public EnumPropertyViewModel(object selectedElement, ObservableCollection<object> values)
    {
        _selectedElement = selectedElement;
        Values = values;
    }
}