using HunterPie.UI.Architecture;

namespace HunterPie.Features.Patches.ViewModels;

public class PatchViewModel : ViewModel
{
    public required string Link { get; init; }
    public required string Banner { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
}