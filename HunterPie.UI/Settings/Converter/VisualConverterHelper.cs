using HunterPie.Core.Architecture;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Converter;

public static class VisualConverterHelper
{
    public static Binding CreateBinding(object parent, string path = nameof(Observable<bool>.Value))
    {
        var binding = new Binding(path)
        {
            Mode = BindingMode.TwoWay,
            Source = parent,
        };

        return binding;
    }
}