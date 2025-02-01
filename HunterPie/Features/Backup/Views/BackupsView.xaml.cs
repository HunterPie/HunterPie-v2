using HunterPie.Features.Backup.ViewModels;
using HunterPie.UI.Architecture;

namespace HunterPie.Features.Backup.Views;
/// <summary>
/// Interaction logic for BackupsView.xaml
/// </summary>
internal partial class BackupsView : View<BackupsViewModel>
{
    public BackupsView()
    {
        InitializeComponent();
    }

    protected override async void Initialize()
    {
        await ViewModel.FetchBackupsAsync();
    }
}