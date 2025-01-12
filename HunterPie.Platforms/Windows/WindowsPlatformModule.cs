using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Platforms.Windows.Process;
using HunterPie.Platforms.Windows.Registry;
using HunterPie.Platforms.Windows.Vault;

namespace HunterPie.Platforms.Windows;

internal class WindowsPlatformModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<WindowsProcessWatcher>()
            .WithSingle<WindowsRegistryAsync>()
            .WithSingle<WindowsCredentialVault>();
    }
}