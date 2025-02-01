using HunterPie.Core.Architecture;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

public class ChatCategoryViewModel : Bindable
{
    private string _name;
    private string _description;
    private ImageSource _icon;

    public string Name { get => _name; set => SetValue(ref _name, value); }
    public string Description { get => _description; set => SetValue(ref _description, value); }
    public ImageSource Icon { get => _icon; set => SetValue(ref _icon, value); }
    public ObservableCollection<ChatElementViewModel> Elements { get; } = new();
}