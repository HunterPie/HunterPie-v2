using System.Windows.Media;
using App = System.Windows.Application;

namespace HunterPie.UI.Assets.Application;

public static class Resources
{

    public static ImageSource Icon(string iconName) => (ImageSource)App.Current.FindResource(iconName);

    public static T Get<T>(string resourceName) => (T)App.Current.FindResource(resourceName);
}