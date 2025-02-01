using HunterPie.Core.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

public class ChatViewModel : Bindable
{
    private bool _isChatOpen;

    public ObservableCollection<ChatCategoryViewModel> Categories { get; } = new();
    public bool IsChatOpen { get => _isChatOpen; set => SetValue(ref _isChatOpen, value); }
}