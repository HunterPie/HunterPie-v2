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

internal class AccountVerificationResendFlowViewModel : ViewModel
{
    private readonly PoogieAccountConnector _accountConnector;

    private bool _isRequestingVerification;
    private bool _canRequestVerification;
    private bool _isFlowActive;
    private string _email = string.Empty;

    public bool IsRequestingVerification
    {
        get => _isRequestingVerification;
        set => SetValue(ref _isRequestingVerification, value);
    }

    public bool CanRequestVerification
    {
        get => _canRequestVerification;
        set => SetValue(ref _canRequestVerification, value);
    }

    public bool IsFlowActive
    {
        get => _isFlowActive;
        set => SetValue(ref _isFlowActive, value);
    }

    public string Email
    {
        get => _email;
        set
        {
            CanRequestVerification = _email.Length > 0;
            SetValue(ref _email, value);
        }
    }

    public AccountVerificationResendFlowViewModel(PoogieAccountConnector accountConnector)
    {
        _accountConnector = accountConnector;
    }

    public async Task RequestAccountVerification()
    {
        IsRequestingVerification = true;

        PoogieResult<RequestAccountVerificationResponse> response = await _accountConnector.RequestAccountVerificationAsync(
            new RequestAccountVerifyRequest(Email: Email)
        );

        IsRequestingVerification = false;

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
                "//Strings/Client/Integrations/Poogie[@Id='ACCOUNT_REGISTER_SUCCESS']"
            ).Replace("{Email}", Email),
            DisplayTime: TimeSpan.FromSeconds(10)
        );
        await NotificationService.Show(successOptions);

        IsFlowActive = false;
        Email = string.Empty;
    }

    public void NavigateToLoginFlow() => IsFlowActive = false;
}