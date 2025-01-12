using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using System;
using System.ComponentModel;

namespace HunterPie.UI.Overlay.Widgets.Classes.Views;

/// <summary>
/// Interaction logic for ClassView.xaml
/// </summary>
public partial class ClassView : View<ClassViewModel>, IWidget<ClassWidgetConfig>, IWidgetWindow, INotifyPropertyChanged
{
    private ClassWidgetConfig _config;

    public WidgetType Type => WidgetType.ClickThrough;

    public IWidgetSettings Settings
    {
        get => _config;
        private set
        {
            _config = (ClassWidgetConfig)value;
            this.N(PropertyChanged);
        }
    }

    public event EventHandler<WidgetType> OnWidgetTypeChange;

    ClassWidgetConfig IWidget<ClassWidgetConfig>.Settings => _config;

    public string Title => "Class Widget";


    public ClassView()
    {
        InitializeComponent();

        // TODO: create interface for widget ViewModels that has a settings property that the view can bind to avoid this hacky solution
        ViewModel.PropertyChanged += OnPropertyChanged;
        Settings = ViewModel.CurrentSettings;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(ClassViewModel.CurrentSettings))
            return;

        Dispatcher.Invoke(() => Settings = null);
        Dispatcher.Invoke(() => Settings = ViewModel.CurrentSettings);
    }

    public event PropertyChangedEventHandler PropertyChanged;
}