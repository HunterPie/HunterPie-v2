using HunterPie.Domain.Interfaces;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NavigationInitializer : IInitializer
{
    private readonly IBodyNavigator _mainBodyNavigator;
    private readonly IAppNavigator _mainNavigator;

    public NavigationInitializer(
        IBodyNavigator mainBodyNavigator,
        IAppNavigator mainNavigator)
    {
        _mainBodyNavigator = mainBodyNavigator;
        _mainNavigator = mainNavigator;
    }

    public Task Init()
    {
        Navigator.SetNavigators(
            body: _mainBodyNavigator,
            app: _mainNavigator
        );

        return Task.CompletedTask;
    }
}