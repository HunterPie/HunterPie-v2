using HunterPie.UI.Architecture;
using HunterPie.UI.Settings;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HunterPie.UI.Navigation;

#nullable enable
public static class NavigationProvider
{
    private static readonly Dictionary<Type, DataTemplate> Templates = new();

    /// <summary>
    /// Binds a view model to an specific view type
    /// </summary>
    /// <typeparam name="TView">Type of the view</typeparam>
    /// <typeparam name="TViewModel">Type of the view's view model</typeparam>
    public static void Bind<TView, TViewModel>()
        where TView : FrameworkElement
        where TViewModel : ViewModel
    {
        DataTemplate dataTemplate = DataTemplateFactory.Create<TView>();
        Templates.Add(typeof(TViewModel), dataTemplate);
    }

    public static DataTemplate? FindBy(Type viewModelType)
    {
        return Templates.GetValueOrDefault(viewModelType);
    }
}