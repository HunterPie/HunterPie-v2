using HunterPie.Features.Patches.ViewModels;
using HunterPie.UI.Architecture;

namespace HunterPie.Features.Patches.Views;
/// <summary>
/// Interaction logic for PatchesView.xaml
/// </summary>
internal partial class PatchesView : View<PatchesViewModel>
{
    public PatchesView()
    {
        InitializeComponent();
    }

    protected override async void Initialize() => await ViewModel.FetchPatchesAsync();
}