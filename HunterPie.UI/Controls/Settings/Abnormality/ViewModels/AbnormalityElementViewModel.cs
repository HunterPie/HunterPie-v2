namespace HunterPie.UI.Controls.Settings.Abnormality.ViewModels;

public class AbnormalityElementViewModel : Architecture.ViewModel
{
    private string _id;
    public string Id { get => _id; set => SetValue(ref _id, value); }

    private string _icon;
    public string Icon { get => _icon; set => SetValue(ref _icon, value); }

    private string _name;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private string _category;
    public string Category { get => _category; set => SetValue(ref _category, value); }

    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    private bool _isMatch;
    public bool IsMatch { get => _isMatch; set => SetValue(ref _isMatch, value); }
}