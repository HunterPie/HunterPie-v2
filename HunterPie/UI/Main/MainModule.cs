using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Main.Navigators;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Main.Views;

namespace HunterPie.UI.Main;

internal class MainModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<MainBodyViewModel>()
            .WithSingle<MainBodyNavigator>()
            .WithSingle<MainViewModel>()
            .WithSingle<MainNavigator>()
            .WithSingle<NavigatorController>()
            .WithSingle(() =>
                new MainView
                {
                    DataContext = registry.Get<MainViewModel>()
                }
            );
    }
}