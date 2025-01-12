using System.Windows.Controls;

namespace HunterPie.UI.Architecture.Extensions;

public static class UserControlExtensions
{
    public static T FindResource<T>(this UserControl self, string name) => (T)self.FindResource(name);
}