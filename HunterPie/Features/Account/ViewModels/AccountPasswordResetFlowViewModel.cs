using HunterPie.Core.Client.Localization;
using HunterPie.Core.Notification;
using HunterPie.Core.Notification.Model;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.ViewModels;

internal class AccountPasswordResetFlowViewModel : ViewModel
{
    private readonly PoogieAccountConnector _accountConnector;

    private bool _isRequestingCode;
    private bool _hasCodeBeenSent;
    private bool _isResetInProgress;
    private bool _canChangePassword;
    private bool _isFlowActive;
    private string _email = string.Empty;
    private string _code = string.Empty;
    private string _password = string.Empty;

    public bool IsRequestingCode { get => _isRequestingCode; set => SetValue(ref _isRequestingCode, value); }
    public bool HasCodeBeenSent { get => _hasCodeBeenSent; set => SetValue(ref _hasCodeBeenSent, value); }
    public bool IsResetInProgress { get => _isResetInProgress; set => SetValue(ref _isResetInProgress, value); }
    public bool CanChangePassword { get => _canChangePassword; set => SetValue(ref _canChangePassword, value); }
    public bool IsFlowActive { get => _isFlowActive; set => SetValue(ref _isFlowActive, value); }
    public string Email { get => _email; set => SetValueAndUpdateState(ref _email, value); }
    public string Code { get => _code; set => SetValueAndUpdateState(ref _code, value); }
    public string Password { get => _password; set => SetValueAndUpdateState(ref _password, value); }

    public AccountPasswordResetFlowViewModel(PoogieAccountConnector accountConnector)
    {
        _accountConnector = accountConnector;
    }

    public async Task RequestResetCodeAsync()
    {
        IsRequestingCode = true;

        PoogieResult<PasswordChangeResponse> response =
            await _accountConnector.ForgotPasswordAsync(new PasswordResetRequest(Email: Email));

        IsRequestingCode = false;

        if (response.Error is { } error)
        {
            var options = new NotificationOptions(
                Type: NotificationType.Error,
                Title: "Error",
                Description: Localization.GetEnumString(error.Code),
                DisplayTime: TimeSpan.FromSeconds(10)
            );
            await NotificationService.Show(options);
            return;
        }

        var successOptions = new NotificationOptions(
            Type: NotificationType.Success,
            Title: "Success",
            Description: Localization.QueryString(
                "//Strings/Client/Integrations/Poogie[@Id='PASSWORD_RESET_EMAIL_STRING']"
            ).Replace("{Email}", Email),
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        await NotificationService.Show(successOptions);
        HasCodeBeenSent = true;
    }

    public async Task ChangePassword()
    {
        IsResetInProgress = true;

        PoogieResult<PasswordChangeResponse> response =
            await _accountConnector.ChangePasswordAsync(new ChangePasswordRequest
            (
                Email: Email,
                Code: Code,
                NewPassword: Password
            ));

        IsResetInProgress = false;

        if (response.Error is { } error)
        {
            var errorOptions = new NotificationOptions(
                Type: NotificationType.Error,
                Title: "Error",
                Description: Localization.GetEnumString(error.Code),
                DisplayTime: TimeSpan.FromSeconds(10)
            );
            await NotificationService.Show(errorOptions);

            return;
        }

        var successOptions = new NotificationOptions(
            Type: NotificationType.Success,
            Title: "Success",
            Description: Localization.QueryString("//Strings/Client/Integrations/Poogie[@Id='PASSWORD_RESET_SUCCESS_STRING']"),
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        await NotificationService.Show(successOptions);

        NavigateToLoginFlow();
    }

    public void NavigateToLoginFlow() => IsFlowActive = false;

    private void SetValueAndUpdateState<T>(ref T field, T value)
    {
        SetValue(ref field, value);
        CanChangePassword = Email.Length > 0 && Code.Length > 0 && Password.Length > 0;
    }
}