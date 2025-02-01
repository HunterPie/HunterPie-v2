using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using LiveCharts.Wpf;

namespace HunterPie.UI.Overlay.Widgets.Damage;

internal class MemberInfo
{
    public PlayerViewModel ViewModel { get; init; }
    public Series Series { get; init; }
    public double JoinedAt { get; set; }
    public double FirstHitAt { get; set; } = -1;
}