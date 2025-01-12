using System;

namespace HunterPie.UI.Navigation;

[Obsolete("Deprecated, use IAppNavigator or IBodyNavigator instead")]
public static class Navigator
{
#pragma warning disable CS8618
    public static INavigator Body;
    public static INavigator App;
#pragma warning restore CS8618

    internal static void SetNavigators(
        IBodyNavigator body,
        IAppNavigator app)
    {
        Body = body;
        App = app;
    }
}