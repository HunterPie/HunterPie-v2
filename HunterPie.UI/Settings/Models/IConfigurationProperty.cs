using HunterPie.Core.Architecture;

namespace HunterPie.UI.Settings.Models;

#nullable enable
public interface IConfigurationProperty
{
    public string Name { get; }
    public string Description { get; }
    public string Group { get; }
    public bool RequiresRestart { get; }
    public Observable<bool>? Condition { get; }
}