using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.Controller;
using HunterPie.Features.Account.Service;
using HunterPie.Features.Account.ViewModels;

namespace HunterPie.Features.Account;

internal class AccountModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<AccountService>()
            .WithSingle<AccountConfig>()
            .WithSingle<RemoteAccountConfigService>()
            .WithSingle<RemoteConfigSyncService>()
            .WithSingle<AccountController>()
            .WithSingle<LocalAccountConfig>();

        registry
            .WithService<AccountLoginFlowViewModel>()
            .WithService<AccountPasswordResetFlowViewModel>()
            .WithService<AccountPreferencesViewModel>()
            .WithService<AccountRegisterFlowViewModel>()
            .WithService<AccountSignFlowViewModel>()
            .WithService<AccountVerificationResendFlowViewModel>();
    }
}