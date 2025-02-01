using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Controls.Paginating.Events;
using HunterPie.UI.Controls.Paginating.Presentation;
using HunterPie.UI.Controls.Paginating.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Button = HunterPie.UI.Controls.Buttons.Button;

namespace HunterPie.UI.Controls.Paginating;
/// <summary>
/// Interaction logic for Pagination.xaml
/// </summary>
public partial class Pagination : UserControl
{
    public static readonly RoutedEvent PageClickEvent = EventManager.RegisterRoutedEvent(
        nameof(PageClick),
        RoutingStrategy.Bubble,
        typeof(PaginationEventHandler),
        typeof(Pagination)
    );

    public event PaginationEventHandler PageClick
    {
        add => AddHandler(PageClickEvent, value);
        remove => RemoveHandler(PageClickEvent, value);
    }

    public ObservableCollection<PaginationItemViewModel> Pages { get; } = CreateBufferedCollection(6);

    public int TotalPages
    {
        get => (int)GetValue(TotalPagesProperty);
        set => SetValue(TotalPagesProperty, value);
    }

    public static readonly DependencyProperty TotalPagesProperty =
        DependencyProperty.Register(nameof(TotalPages), typeof(int), typeof(Pagination), new PropertyMetadata(0, OnPagesChange));

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(Pagination), new PropertyMetadata(0, OnPagesChange));

    public bool CanPaginatePrevious => (bool)GetValue(CanPaginatePreviousProperty);

    public static readonly DependencyProperty CanPaginatePreviousProperty =
        DependencyProperty.Register(nameof(CanPaginatePrevious), typeof(bool), typeof(Pagination));

    public bool CanPaginateNext => (bool)GetValue(CanPaginateNextProperty);

    public static readonly DependencyProperty CanPaginateNextProperty =
        DependencyProperty.Register(nameof(CanPaginateNext), typeof(bool), typeof(Pagination));

    public Pagination()
    {
        InitializeComponent();
    }

    private static void OnPagesChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Pagination pagination)
            return;

        int totalPages = pagination.TotalPages;
        int currentPage = pagination.CurrentPage;
        int pageBase = currentPage <= 1 ? 2
            : currentPage >= (totalPages - 2) ? totalPages - 3
            : currentPage;

        Span<int> availablePages = stackalloc int[5] { 0, pageBase - 1, pageBase, pageBase + 1, totalPages - 1 };

        int lastPageAdded = int.MinValue;
        int currentPageIndex = 0;
        foreach (int page in availablePages)
        {
            if (page < 0 || page >= totalPages || page == lastPageAdded)
                continue;

            pagination.Pages[currentPageIndex].Apply(it =>
            {
                it.IsVisible = true;
                it.IsActive = page == currentPage;
                it.Page = page + 1;
            });

            currentPageIndex++;
            lastPageAdded = page;
        }

        pagination.SetValue(CanPaginateNextProperty, currentPage < totalPages - 1);
        pagination.SetValue(CanPaginatePreviousProperty, currentPage > 0);

        for (int i = currentPageIndex; i < pagination.Pages.Count; i++)
            pagination.Pages[i].Apply(it => it.IsVisible = false);

    }

    private void OnPageButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { DataContext: PaginationItemViewModel vm })
            return;

        RaiseEvent(new PaginationEventArgs(PageClickEvent, this, vm.Page - 1));
    }

    private void OnPreviousPageClick(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new PaginationEventArgs(PageClickEvent, this, CurrentPage - 1));
    }

    private void OnNextPageClick(object sender, RoutedEventArgs e)
    {
        RaiseEvent(new PaginationEventArgs(PageClickEvent, this, CurrentPage + 1));
    }

    private static ObservableCollection<PaginationItemViewModel> CreateBufferedCollection(int count)
    {
        IEnumerable<PaginationItemViewModel> collection = Enumerable.Range(0, count)
            .Select((_) => new PaginationItemViewModel());

        return new ObservableCollection<PaginationItemViewModel>(collection);
    }
}