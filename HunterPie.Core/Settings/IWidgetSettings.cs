using HunterPie.Core.Architecture;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Settings;

public interface IWidgetSettings
{
    Observable<bool> Initialize { get; set; }
    Observable<bool> Enabled { get; set; }
    Observable<bool> HideWhenUiOpen { get; set; }
    Position Position { get; set; }
    Range Opacity { get; set; }
    Range Scale { get; set; }
    Observable<bool> StreamerMode { get; set; }
}