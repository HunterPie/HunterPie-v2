using HunterPie.Core.Architecture;
using HunterPie.DI;
using HunterPie.UI.Overlay;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HunterPie.UI.Architecture;

// TODO: Refactor this
public class View<TViewModel> : UserControl, IDisposable, IView<TViewModel>
    where TViewModel : Bindable
{
    public TViewModel ViewModel => (TViewModel)DataContext;

    public Dispatcher UIThread => Application.Current.Dispatcher;

    protected virtual TViewModel InitializeViewModel(params object[] args)
    {
        try
        {
            if (this is not IWidgetWindow)
                return Activator.CreateInstance<TViewModel>();
        }
        catch
        {
            return DependencyContainer.Get<TViewModel>();
        }

        try
        {
            return (TViewModel)Activator.CreateInstance(typeof(TViewModel), args);
        }
        catch { }

        return Activator.CreateInstance<TViewModel>();
    }
    protected bool IsDesignMode => DesignerProperties.GetIsInDesignMode(this);

    public View()
    {
        DataContext = InitializeViewModel();
    }

    public View(params object[] args)
    {
        DataContext = InitializeViewModel(args);
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (IsDesignMode)
            return;

        Unloaded += OnViewUnloaded;

        Initialize();
    }

    private void OnViewUnloaded(object sender, RoutedEventArgs e)
    {
        if (ViewModel is IDisposable vm)
            vm.Dispose();

        Unloaded -= OnViewUnloaded;

        Dispose();
    }

    protected virtual void Initialize() { }

    public virtual void Dispose() { }
}