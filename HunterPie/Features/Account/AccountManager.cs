using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.Core.Vault;
using HunterPie.Core.Vault.Model;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account;

[Obsolete("Use IAccountUseCase instead")]
internal class AccountManager : IEventDispatcher
{
    private UserAccount? _cachedAccount;
    private readonly PoogieAccountConnector _accountConnector = null;
    private static readonly AccountManager Instance = new();

    public static event EventHandler<AccountLoginEventArgs>? OnSignIn;
    public static event EventHandler<AccountLoginEventArgs>? OnSessionStart;
    public static event EventHandler<EventArgs>? OnSignOut;
    public static event EventHandler<AccountAvatarEventArgs>? OnAvatarChange;

    public static bool IsLoggedIn() => GetSessionToken() is not null;

    public static async Task<bool> ValidateSessionToken()
    {
        if (await FetchAccount() is not null)
            return GetSessionToken() is not null;

        CredentialVaultService.DeleteCredential();
        Instance.Dispatch(OnSignOut);
        return false;

    }

    public static async Task<PoogieResult<LoginResponse>?> Login(LoginRequest request)
    {
        (string title, string description) =
            Localization.Resolve("//Strings/Client/Integrations/Poogie[@Id='SIGN_IN_NOTIFICATION']");

        var progressNotification = new NotificationOptions(
            Type: NotificationType.InProgress,
            Title: title,
            Description: description,
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        Guid notificationId = await NotificationService.Show(progressNotification);

        PoogieResult<LoginResponse> loginResponse = await Instance._accountConnector.LoginAsync(request);

        if (loginResponse.Error is { } err)
        {
            NotificationOptions errorNotification = progressNotification with
            {
                Type = NotificationType.Error,
                Description = Localization.GetEnumString(err.Code)
            };
            NotificationService.Update(notificationId, errorNotification);

            return null;
        }

        if (loginResponse.Response is not { } response)
            return null;

        CredentialVaultService.SaveCredential(request.Email, response.Token);

        UserAccount? account = await FetchAccount();

        if (account is null)
            return null;

        NotificationOptions successOptions = progressNotification with
        {
            Type = NotificationType.Success,
            Description = Localization.QueryString("//Strings/Client/Integrations/Poogie[@Id='LOGIN_SUCCESS']")
                .Replace("{Username}", account.Username)
        };
        NotificationService.Update(notificationId, successOptions);

        Instance.Dispatch(OnSignIn, new AccountLoginEventArgs { Account = account });

        return loginResponse;
    }

    public static async void Logout()
    {
        _ = await Instance._accountConnector.LogoutAsync();

        CredentialVaultService.DeleteCredential();
        Instance._cachedAccount = null;

        Instance.Dispatch(OnSignOut);
    }

    public static string? GetSessionToken()
    {
        Credential? credential = CredentialVaultService.GetCredential();

        return credential?.Password;
    }

    public static async Task UploadAvatar(string path)
    {
        var notificationOptions = new NotificationOptions(
            Type: NotificationType.InProgress,
            Title: "UploadAsync",
            Description: "Uploading profile picture...",
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        Guid notificationId = await NotificationService.Show(notificationOptions);

        PoogieResult<MyUserAccountResponse> account = await Instance._accountConnector.UploadAvatarAsync(path);

        if (account.Error is { } error)
        {
            NotificationOptions errorOptions = notificationOptions with
            {
                Type = NotificationType.Error,
                Description = Localization.GetEnumString(error.Code),
                DisplayTime = TimeSpan.FromSeconds(10)
            };
            NotificationService.Update(notificationId, errorOptions);

            return;
        }

        NotificationOptions successOptions = notificationOptions with
        {
            Type = NotificationType.Success,
            Description = Localization.QueryString("//Strings/Client/Integrations/Poogie[@Id='AVATAR_UPLOAD_SUCCESS']"),
        };
        NotificationService.Update(notificationId, successOptions);

        Instance._cachedAccount = account.Response!.ToModel();

        Instance.Dispatch(OnAvatarChange, new AccountAvatarEventArgs { AvatarUrl = Instance._cachedAccount.AvatarUrl });
    }

    public static async Task<UserAccount?> FetchAccount()
    {
        Credential? credential = CredentialVaultService.GetCredential();

        if (credential is null)
            return null;

        if (Instance._cachedAccount is { } cached)
            return cached;

        PoogieResult<MyUserAccountResponse> result = await Instance._accountConnector.MyUserAccountAsync();

        if (result.Response is not { } account)
            return null;

        Instance._cachedAccount = account.ToModel();

        Instance.Dispatch(OnSessionStart, new AccountLoginEventArgs { Account = Instance._cachedAccount });

        return Instance._cachedAccount;
    }
}