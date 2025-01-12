using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Features;
using HunterPie.Core.Extensions;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.UI.Settings.Converter;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using ConfigurationPropertyAttribute = HunterPie.Core.Settings.Annotations.ConfigurationPropertyAttribute;

namespace HunterPie.UI.Settings;

#nullable enable
public class ConfigurationAdapter
{
    private const string DEFAULT_SETTING_LOCALIZATION_PATH = "//Strings/Client/Settings/Setting[@Id='{0}']";
    private const string DEFAULT_CONFIGURATION_GROUP_PATH = "//Strings/Client/ConfigurationGroups/Group[@Id='{0}']";

    public static ObservableCollection<ConfigurationCategory> Adapt<T>(T configuration, GameProcessType game = GameProcessType.None)
    {
        Type configurationType = typeof(T);
        ConfigurationCategory[] categories = BuildCategoryParent(configurationType, configuration, game);

        return new ObservableCollection<ConfigurationCategory>(categories);
    }

    private static ConfigurationCategory[] BuildCategoryParent(Type parentType, object? parent, GameProcessType game)
    {
        ConfigurationAttribute? configurationAttribute = parentType.GetCustomAttribute<ConfigurationAttribute>();

        if (configurationAttribute is { })
            return BuildCategory(parentType, parent, game);

        PropertyInfo[] parentProperties = parentType.GetProperties();

        ConfigurationCategory[] AdaptCategory(PropertyInfo property)
        {
            return BuildCategory(
                categoryType: property.PropertyType,
                category: property.GetValue(parent),
                game: game
            );
        }

        return parentProperties.SelectMany(AdaptCategory)
                               .ToArray();
    }

    private static ConfigurationCategory[] BuildCategory(Type categoryType, object? category, GameProcessType game)
    {
        if (category is null)
            return Array.Empty<ConfigurationCategory>();

        if (!categoryType.GetInterfaces().Contains(typeof(ISettings)))
            return Array.Empty<ConfigurationCategory>();

        ConfigurationAttribute? configurationAttribute = categoryType.GetCustomAttribute<ConfigurationAttribute>();

        if (configurationAttribute is not { })
            return BuildCategoryParent(categoryType, category, game);

        if (configurationAttribute.DependsOnFeature is { } featureFlag
            && !FeatureFlagManager.IsEnabled(featureFlag))
            return Array.Empty<ConfigurationCategory>();

        if (!configurationAttribute.AvailableGames.HasFlag(game))
            return Array.Empty<ConfigurationCategory>();

        (string name, string description) =
            Localization.Resolve(DEFAULT_SETTING_LOCALIZATION_PATH.Format(configurationAttribute.Name));

        List<ConfigurationCategory> categories = new();
        List<IConfigurationProperty> configurationProperties = new();
        PropertyInfo[] properties = categoryType.GetProperties();
        Observable<bool>? conditionalConfiguration = null;

        foreach (PropertyInfo property in properties)
        {
            Type propertyType = property.PropertyType;
            object? propertyValue = property.GetValue(category);
            ConfigurationPropertyAttribute? propertyAttribute = property.GetCustomAttribute<ConfigurationPropertyAttribute>();

            if (propertyAttribute is not { })
            {
                ConfigurationCategory[] subCategories = BuildCategory(propertyType, propertyValue, game);
                categories.AddRange(subCategories);
                continue;
            }

            if (!propertyAttribute.AvailableGames.HasFlag(game))
                continue;

            if (propertyValue is null)
                continue;

            (string propertyName, string propertyDescription) =
                    Localization.Resolve(DEFAULT_SETTING_LOCALIZATION_PATH.Format(propertyAttribute.Name));
            string groupName = Localization.QueryString(DEFAULT_CONFIGURATION_GROUP_PATH.Format(propertyAttribute.Group));

            GameConfigurationAdapterAttribute? adapterAttribute =
                property.GetCustomAttribute<GameConfigurationAdapterAttribute>();

            var data = new PropertyData(
                Name: propertyName,
                Description: propertyDescription,
                Group: groupName,
                Value: propertyValue,
                Adapter: adapterAttribute?.Adapter,
                Condition: conditionalConfiguration,
                RequiresRestart: propertyAttribute.RequiresRestart
            );
            if (BuildProperty(propertyType, data, game) is not { } configurationProperty)
                continue;

            configurationProperties.Add(configurationProperty);

            if (property.GetCustomAttribute<ConfigurationConditionAttribute>() is { }
                && propertyValue is Observable<bool> condition)
                conditionalConfiguration = condition;
        }

        ObservableCollection<ConfigurationGroup> observableGroups =
            new(GroupProperties(configurationProperties));

        categories.Add(
            item: new ConfigurationCategory(
                Name: name,
                Description: description,
                Icon: configurationAttribute.Icon,
                Groups: observableGroups
            )
        );

        return categories.ToArray();
    }

    private static IConfigurationProperty? BuildProperty(
        Type type,
        PropertyData data,
        GameProcessType game
    )
    {
        IConfigurationPropertyBuilder? builder = ConfigurationPropertyProvider.FindBy(type);

        return builder?.Build(data, game);
    }

    private static IEnumerable<ConfigurationGroup> GroupProperties(IEnumerable<IConfigurationProperty> configurationProperties)
    {
        IEnumerable<IGrouping<string, IConfigurationProperty>> groupedProperties = configurationProperties.GroupBy((it) => it.Group);

        return groupedProperties.Select(group =>
            new ConfigurationGroup(
                Name: group.Key,
                Properties: new ObservableCollection<IConfigurationProperty>(group)
            )
        );
    }
}