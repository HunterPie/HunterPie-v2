using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HunterPie.UI.Architecture.Extensions;

internal static class INotifyPropertyChangedExtensions
{
    public static void N(this INotifyPropertyChanged self, PropertyChangedEventHandler @event, [CallerMemberName] string propertyName = "") => @event?.Invoke(self, new PropertyChangedEventArgs(propertyName));
}