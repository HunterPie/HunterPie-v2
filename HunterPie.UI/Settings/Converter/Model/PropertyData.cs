using HunterPie.Core.Architecture;
using HunterPie.Core.Settings.Adapters;

namespace HunterPie.UI.Settings.Converter.Model;

#nullable enable
public record PropertyData(
    string Name,
    string Description,
    string Group,
    object Value,
    bool RequiresRestart,
    Observable<bool>? Condition,
    ISettingAdapter? Adapter
);