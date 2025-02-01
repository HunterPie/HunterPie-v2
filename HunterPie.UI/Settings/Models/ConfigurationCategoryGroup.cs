using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.Models;

/// <summary>
/// Group of categories
/// </summary>
/// <param name="Name">Name of the group</param>
/// <param name="Categories">Categories</param>
public record ConfigurationCategoryGroup(
    string Name,
    ObservableCollection<ConfigurationCategory> Categories
);