using HunterPie.Core.Architecture;
using Newtonsoft.Json;
using System.ComponentModel;

namespace HunterPie.Core.Domain.Features.Domain;

public class Feature : IFeature
{
    public Observable<bool> IsEnabled { get; } = false;

    [JsonConstructor]
    public Feature()
    {
        IsEnabled.PropertyChanged += OnPropertyChange;
    }

    public Feature(bool defaultInitializer = false)
    {
        IsEnabled = defaultInitializer;
        IsEnabled.PropertyChanged += OnPropertyChange;
    }

    ~Feature()
    {
        IsEnabled.PropertyChanged -= OnPropertyChange;
    }

    private void OnPropertyChange(object sender, PropertyChangedEventArgs e)
    {
        if (IsEnabled)
            OnEnable();
        else
            OnDisable();
    }

    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
}