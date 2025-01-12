using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.Models;

/// <summary>
/// Groups of properties of a given category
/// </summary>
/// <param name="Name">Name of the group</param>
/// <param name="Properties">Properties part of given group</param>
public record ConfigurationGroup(
    string Name,
    ObservableCollection<IConfigurationProperty> Properties
);