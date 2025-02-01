using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Cache;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.Core.Vault;
using HunterPie.Core.Vault.Model;
using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Service;

internal class AccountService : IAccountUseCase, IEventDispatcher
{
    private const string ACCOUNT_CACHE_KEY = "account::key";
    private readonly ICredentialVault _credentialVault;
    private readonly IAsyncCache _cache;
    private readonly PoogieAccountConnector _accountConnector;

    public event EventHandler<AccountLoginEventArgs>? SignIn;
    public event EventHandler<EventArgs>? SignOut;
    public event EventHandler<AccountLoginEventArgs>? SessionStart;
    public event EventHandler<AccountAvatarEventArgs>? AvatarChange;

    public AccountService(
        ICredentialVault credentialVault,
        IAsyncCache cache,
        PoogieAccountConnector accountConnector
    )
    {
        _credentialVault = credentialVault;
        _cache = cache;
        _accountConnector = accountConnector;
    }

    public async Task<UserAccount?> GetAsync()
    {
        Credential? credential = _credentialVault.Get();

        if (credential is not { })
            return null;

        if (await _cache.GetAsync<UserAccount>(ACCOUNT_CACHE_KEY) is { } cachedAccount)
            return cachedAccount;

        PoogieResult<MyUserAccountResponse> result = await _accountConnector.MyUserAccountAsync();

        if (result.Response is not { } account)
            return null;

        UserAccount model = account.ToModel();
        await _cache.SetAsync(ACCOUNT_CACHE_KEY, model);

        this.Dispatch(
            toDispatch: SignIn,
            data: new AccountLoginEventArgs
            {
                Account = model
            }
        );

        return model;
    }

    public async Task<bool> IsValidSessionAsync()
    {
        if (await GetAsync() is { })
            return true;

        _credentialVault.Delete();
        this.Dispatch(toDispatch: SignOut);
        return false;
    }

    public async Task<PoogieResult<LoginResponse>?> LoginAsync(LoginRequest request)
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

        PoogieResult<LoginResponse> loginResponse = await _accountConnector.LoginAsync(request);

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

        _credentialVault.Create(
            username: request.Email,
            password: response.Token
        );

        UserAccount? account = await GetAsync();

        if (account is null)
            return null;

        NotificationOptions successOptions = progressNotification with
        {
            Type = NotificationType.Success,
            Description = Localization.QueryString("//Strings/Client/Integrations/Poogie[@Id='LOGIN_SUCCESS']")
                .Replace("{Username}", account.Username)
        };
        NotificationService.Update(notificationId, successOptions);

        this.Dispatch(SignIn, new AccountLoginEventArgs { Account = account });

        return loginResponse;
    }

    public async Task UploadAvatarAsync(string path)
    {
        var notificationOptions = new NotificationOptions(
            Type: NotificationType.InProgress,
            Title: "Upload",
            Description: "Uploading profile picture...",
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        Guid notificationId = await NotificationService.Show(notificationOptions);

        PoogieResult<MyUserAccountResponse> account = await _accountConnector.UploadAvatarAsync(path);

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

        UserAccount? model = account.Response?.ToModel();

        if (model is not { })
            return;

        await _cache.SetAsync(ACCOUNT_CACHE_KEY, model);

        this.Dispatch(AvatarChange, new AccountAvatarEventArgs { AvatarUrl = model.AvatarUrl });
    }

    public async Task LogoutAsync()
    {
        _ = await _accountConnector.LogoutAsync();

        _credentialVault.Delete();

        await _cache.ClearAsync(ACCOUNT_CACHE_KEY);

        this.Dispatch(SignOut);
    }

    private string? GetSessionToken()
    {
        return _credentialVault.Get()?.Password;
    }
}