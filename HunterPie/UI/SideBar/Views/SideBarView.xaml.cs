using HunterPie.Core.Architecture;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.SideBar.Views;

/// <summary>
/// Interaction logic for SideBarView.xaml
/// </summary>
public partial class SideBarView : UserControl
{
    public Observable<bool> IsMouseInside { get; } = false;

    public SideBarView()
    {
        InitializeComponent();
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsMouseInside.Value = false;
        PART_SideBar.IsHitTestVisible = false;
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        IsMouseInside.Value = true;
        PART_SideBar.IsHitTestVisible = true;
    }
}