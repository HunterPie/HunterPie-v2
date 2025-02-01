using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Core.Settings.Common;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels.Internal;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings;

public static class FeatureFlagAdapter
{
    private const string FEATURES_FLAG_XPATH = "//Strings/Client/Settings/Setting[@Id='FEATURE_FLAGS_STRING']";
    private const string GENERAL_GROUP_XPATH = $"//Strings/Client/ConfigurationGroups/Group[@Id='{CommonConfigurationGroups.GENERAL}']";
    private const string FEATURE_FLAG_GROUP_XPATH = $"//Strings/Client/ConfigurationGroups/Group[@Id='{CommonConfigurationGroups.FEATURE_FLAGS}']";

    public static ObservableCollection<ConfigurationCategoryGroup> Adapt(Dictionary<string, IFeature> flags)
    {
        ObservableCollection<IConfigurationProperty> properties = new();
        string categoryName = Localization.QueryString(GENERAL_GROUP_XPATH);

        foreach ((string name, IFeature feature) in flags)
        {
            var property = new BooleanPropertyViewModel(feature.IsEnabled)
            {
                Name = name,
                Description = "",
                Group = categoryName,
                RequiresRestart = true,
            };

            properties.Add(property);
        }

        var group = new ConfigurationGroup(
            Name: categoryName,
            Properties: properties
        );

        (string groupName, string groupDescription) = Localization.Resolve(FEATURES_FLAG_XPATH);

        string categoryGroup = Localization.QueryString(FEATURE_FLAG_GROUP_XPATH);

        var category = new ConfigurationCategory(
            Name: groupName,
            Description: groupDescription,
            Icon: "ICON_FLAG",
            CategoryGroup: categoryGroup,
            Groups: new ObservableCollection<ConfigurationGroup> { group }
        );

        return new ObservableCollection<ConfigurationCategoryGroup>
        {
            new (
                Name: categoryGroup,
                Categories: new ObservableCollection<ConfigurationCategory> { category }
            )
        };
    }
}