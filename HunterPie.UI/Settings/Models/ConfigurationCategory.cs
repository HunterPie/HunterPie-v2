using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.Models;

/// <summary>
/// Configuration category with the groups of that given category
/// </summary>
/// <param name="Name">Name of the category</param>
/// <param name="Description">Description of the category</param>
/// <param name="Icon">Icon for the category</param>
/// <param name="Groups">Property groups of this category</param>
public record ConfigurationCategory(
    string Name,
    string Description,
    string Icon,
    ObservableCollection<ConfigurationGroup> Groups
);