using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Statistics.Details.Views;
/// <summary>
/// Interaction logic for MonsterDetailsView.xaml
/// </summary>
public partial class MonsterDetailsView : UserControl, IView<MonsterDetailsViewModel>
{
    public MonsterDetailsViewModel ViewModel => (MonsterDetailsViewModel)DataContext;

    public MonsterDetailsView()
    {
        InitializeComponent();
    }

    public void InitializeView()
    {
        // HACK: LiveCharts has a pretty awful support for sections
        PART_Graph.AxisX[0].Sections?.Clear();

        ViewModel.SetupView();
    }

    private void OnPlayerClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: PartyMemberDetailsViewModel vm })
            ViewModel.ToggleMember(vm);
    }

    private void OnAbnormalityClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: AbnormalityDetailsViewModel vm })
            ViewModel.ToggleSections(vm);


        ForceGraphRender();
    }

    private void OnMonsterStatusClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: StatusDetailsViewModel vm })
            ViewModel.ToggleSections(vm);

        ForceGraphRender();
    }

    private void ForceGraphRender()
    {
        // HACK: For some reason adding Sections to the collection will not update the graph
        PART_Graph.Update(true);
    }
}