using HunterPie.Core.Architecture;

namespace HunterPie.UI.Architecture;

internal interface IView<out TViewModel> where TViewModel : Bindable
{
    TViewModel ViewModel { get; }
}