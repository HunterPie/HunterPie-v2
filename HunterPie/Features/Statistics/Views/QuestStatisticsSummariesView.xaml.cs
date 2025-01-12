using HunterPie.Core.System;
using HunterPie.Features.Statistics.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Controls.Paginating.Events;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Statistics.Views;

/// <summary>
/// Interaction logic for QuestStatisticsSummariesView.xaml
/// </summary>
internal partial class QuestStatisticsSummariesView : View<QuestStatisticsSummariesViewModel>
{
    private const string PATREON_LINK = "https://www.patreon.com/HunterPie";

    public QuestStatisticsSummariesView()
    {
        InitializeComponent();
    }

    protected override void Initialize() => ViewModel.FetchQuests();

    private void OnSupporterPromptClick(object sender, RoutedEventArgs e) => BrowserService.OpenUrl(PATREON_LINK);

    private void OnSummaryClick(object sender, RoutedEventArgs e)
    {
        if (sender is IView<QuestStatisticsSummaryViewModel> view && view.ViewModel.UploadId is { } uploadId)
            ViewModel.NavigateToHuntDetails(uploadId);
    }

    private void OnPageClick(object sender, PaginationEventArgs e)
    {
        ViewModel.CurrentPage = e.Page;
    }

    private void OnLimitSelectionChange(object sender, SelectionChangedEventArgs e)
    {
        bool mustForceFetch = ViewModel.CurrentPage == 0;

        ViewModel.CurrentPage = 0;

        if (!mustForceFetch)
            return;

        ViewModel.FetchQuests();
    }
}