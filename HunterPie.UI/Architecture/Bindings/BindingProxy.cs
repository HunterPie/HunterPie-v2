using System.Windows;

namespace HunterPie.UI.Architecture.Bindings;

public class BindingProxy : Freezable
{
    public object Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy));

    protected override Freezable CreateInstanceCore()
    {
        return new BindingProxy();
    }
}