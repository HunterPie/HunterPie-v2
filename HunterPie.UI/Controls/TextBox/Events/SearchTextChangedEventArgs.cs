using System;

namespace HunterPie.UI.Controls.TextBox.Events;

public class SearchTextChangedEventArgs : EventArgs
{
    public string Text { get; }

    public SearchTextChangedEventArgs(string text)
    {
        Text = text;
    }
}