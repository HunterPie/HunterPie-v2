﻿using HunterPie.GUI.Parts.Account.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Account.Views;

/// <summary>
/// Interaction logic for AccountPreferencesView.xaml
/// </summary>
public partial class AccountPreferencesView : UserControl
{
    public AccountPreferencesView()
    {
        InitializeComponent();
    }

    private void OnAvatarUploadClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AccountPreferencesViewModel vm)
            return;

        vm.UploadAvatar();
    }
}
