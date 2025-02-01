using HunterPie.UI.Architecture.Events;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Monsters.Views;
/// <summary>
/// Interaction logic for MonsterConfigurationView.xaml
/// </summary>
public partial class MonsterConfigurationView : UserControl
{
    public static readonly RoutedEvent DeleteClickEvent =
        EventManager.RegisterRoutedEvent(
            nameof(DeleteClick),
            RoutingStrategy.Bubble,
            typeof(DataRoutedEventHandler<MonsterConfigurationViewModel>),
            typeof(MonsterConfigurationView)
        );

    public event DataRoutedEventHandler<MonsterConfigurationViewModel> DeleteClick
    {
        add => AddHandler(DeleteClickEvent, value);
        remove => RemoveHandler(DeleteClickEvent, value);
    }

    public MonsterConfigurationView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterConfigurationViewModel vm)
            return;

        vm.IsEditing = !vm.IsEditing;
    }

    private void OnDeleteClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MonsterConfigurationViewModel vm)
            return;

        RaiseEvent(new DataRoutedEventArgs<MonsterConfigurationViewModel>(DeleteClickEvent, this, vm));
    }
}