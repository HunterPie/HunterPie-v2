using HunterPie.Features.Account.UseCase;
using HunterPie.UI.Architecture;
using Microsoft.Win32;
using System;

namespace HunterPie.Features.Account.ViewModels;

internal class AccountPreferencesViewModel : ViewModel
{
    private readonly IAccountUseCase _accountUseCase;

    private string _username = string.Empty;
    public string Username { get => _username; set => SetValue(ref _username, value); }

    private string _email = string.Empty;
    public string Email { get => _email; set => SetValue(ref _email, value); }

    private string _avatarUrl = "https://cdn.hunterpie.com/avatars/default.png";
    public string AvatarUrl { get => _avatarUrl; set => SetValue(ref _avatarUrl, value); }

    private bool _isSupporter;
    public bool IsSupporter { get => _isSupporter; set => SetValue(ref _isSupporter, value); }

    private bool _isFetchingAccount;
    public bool IsFetchingAccount { get => _isFetchingAccount; set => SetValue(ref _isFetchingAccount, value); }

    private bool _isUploadingAvatar;
    public bool IsUploadingAvatar { get => _isUploadingAvatar; set => SetValue(ref _isUploadingAvatar, value); }

    public AccountPreferencesViewModel(IAccountUseCase accountUseCase)
    {
        _accountUseCase = accountUseCase;
    }

    public async void UploadAvatar()
    {
        var dialog = new OpenFileDialog
        {
            InitialDirectory = Environment.CurrentDirectory,
            Filter = "Image Files|*.png;",
            RestoreDirectory = true
        };

        if (dialog.ShowDialog() != true)
            return;

        IsUploadingAvatar = true;
        await _accountUseCase.UploadAvatarAsync(dialog.FileName);
        IsUploadingAvatar = false;
    }
}