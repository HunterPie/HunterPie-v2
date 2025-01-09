﻿using HunterPie.Core.Domain.Cache;
using HunterPie.Core.System.Windows.Registry;
using HunterPie.Core.System.Windows.Vault;
using HunterPie.DI.Module;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Settings;
using System.Windows;

namespace HunterPie.DI.Modules;

internal class HunterPieModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        // Intrinsic
        registry
            .WithSingle(() => Application.Current.Dispatcher)
            .WithService<PoogieConnector>()
            .WithService<InMemoryAsyncCache>()
            .WithService<WindowsCredentialVault>()
            .WithSingle<WindowsRegistry>();

        // Connectors
        registry
            .WithService<PoogieAccountConnector>()
            .WithService<PoogieClientSettingsConnector>();
    }
}