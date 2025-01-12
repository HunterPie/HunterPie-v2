using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

public class ChatElementViewModel : Bindable
{
    private string _text;
    private string _author;
    private int _index;

    public string Text { get => _text; set => SetValue(ref _text, value); }
    public string Author { get => _author; set => SetValue(ref _author, value); }
    public int Index { get => _index; set => SetValue(ref _index, value); }
}