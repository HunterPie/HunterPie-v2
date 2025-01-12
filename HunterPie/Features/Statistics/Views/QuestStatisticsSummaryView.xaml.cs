using HunterPie.Features.Statistics.ViewModels;
using HunterPie.UI.Architecture;

namespace HunterPie.Features.Statistics.Views;
/// <summary>
/// Interaction logic for QuestStatisticsSummary.xaml
/// </summary>
public partial class QuestStatisticsSummaryView : ClickableControl, IView<QuestStatisticsSummaryViewModel>
{
    public QuestStatisticsSummaryViewModel ViewModel => (QuestStatisticsSummaryViewModel)DataContext;

    public QuestStatisticsSummaryView()
    {
        InitializeComponent();
    }
}