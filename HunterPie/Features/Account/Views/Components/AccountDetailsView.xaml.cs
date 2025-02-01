using HunterPie.Features.Account.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Account.Views.Components;
/// <summary>
/// Interaction logic for AccountDetailsView.xaml
/// </summary>
public partial class AccountDetailsView : UserControl
{
    private AccountPreferencesViewModel ViewModel => (AccountPreferencesViewModel)DataContext;

    public AccountDetailsView()
    {
        InitializeComponent();
    }

    private void OnAvatarUploadClick(object sender, RoutedEventArgs e) => ViewModel.UploadAvatar();
}