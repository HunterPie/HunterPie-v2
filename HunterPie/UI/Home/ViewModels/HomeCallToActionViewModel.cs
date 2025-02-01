using HunterPie.UI.Architecture;
using System;
using System.Windows.Media;

namespace HunterPie.UI.Home.ViewModels;

internal class HomeCallToActionViewModel : ViewModel
{
    public ImageSource Icon { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public Action Execute { get; init; }

    public HomeCallToActionViewModel(ImageSource icon, string title, Action execute, string description)
    {
        Icon = icon;
        Title = title;
        Execute = execute;
        Description = description;
    }
}