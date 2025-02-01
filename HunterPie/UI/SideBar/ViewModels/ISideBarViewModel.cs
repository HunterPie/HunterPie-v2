using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

public interface ISideBarViewModel : INotifyPropertyChanged
{
    public Type? Type { get; }
    public string Label { get; }
    public string Icon { get; }
    public bool IsAvailable { get; }
    public bool IsSelected { get; set; }

    public Task ExecuteAsync();
}