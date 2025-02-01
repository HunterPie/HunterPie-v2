using System.Collections.ObjectModel;

namespace HunterPie.UI.Controls.Settings.Abnormality.ViewModels;

public class AbnormalityCategoryViewModel : Architecture.ViewModel
{
    private string _name;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private string _description;
    public string Description { get => _description; set => SetValue(ref _description, value); }

    private string? _icon;
    public string? Icon { get => _icon; set => SetValue(ref _icon, value); }

    public ObservableCollection<AbnormalityElementViewModel> Elements { get; init; }
}