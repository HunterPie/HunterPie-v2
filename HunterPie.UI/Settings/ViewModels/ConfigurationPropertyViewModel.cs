using HunterPie.Core.Architecture;
using HunterPie.UI.Architecture;
using HunterPie.UI.Settings.Models;

namespace HunterPie.UI.Settings.ViewModels;

#nullable enable
public class ConfigurationPropertyViewModel : ViewModel, IConfigurationProperty
{
    private bool _isMatch = true;

    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Group { get; init; } = string.Empty;
    public bool RequiresRestart { get; init; }
    public Observable<bool>? Condition { get; init; } = null;

    public bool IsMatch { get => _isMatch; set => SetValue(ref _isMatch, value); }
}