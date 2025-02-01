using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Adapter;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

#nullable enable
public class MonsterConfigurationViewModel : ViewModel
{
    public required string Name { get; init; }

    public required GameType GameType { get; init; }

    private string? _icon;
    public string? Icon { get => _icon; private set => SetValue(ref _icon, value); }

    public required MonsterConfiguration Configuration { get; init; }

    private bool _isEditing;
    public bool IsEditing { get => _isEditing; set => SetValue(ref _isEditing, value); }

    private bool _isOverriding;
    public bool IsOverriding { get => _isOverriding; set => SetValue(ref _isOverriding, value); }

    private bool _isMatch = true;
    public bool IsMatch { get => _isMatch; set => SetValue(ref _isMatch, value); }

    public async void FetchIcon()
    {
        Icon = await MonsterIconAdapter.UriFrom(GameType, Configuration.Id);
    }
}