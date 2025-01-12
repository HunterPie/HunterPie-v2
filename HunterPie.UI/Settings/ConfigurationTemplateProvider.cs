using HunterPie.UI.Settings.ViewModels.Internal;
using HunterPie.UI.Settings.Views;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HunterPie.UI.Settings;

#nullable enable
public static class ConfigurationTemplateProvider
{
    private static readonly Dictionary<Type, DataTemplate> Templates = new()
    {
        { typeof(BooleanPropertyViewModel), DataTemplateFactory.Create<BooleanConfigurationPropertyView>() },
        { typeof(EnumPropertyViewModel), DataTemplateFactory.Create<EnumConfigurationPropertyView>() },
        { typeof(FileSelectorPropertyViewModel), DataTemplateFactory.Create<FileSelectorConfigurationPropertyView>() },
        { typeof(KeybindingPropertyViewModel), DataTemplateFactory.Create<KeybindingConfigurationPropertyView>() },
        { typeof(PositionPropertyViewModel), DataTemplateFactory.Create<PositionConfigurationPropertyView>() },
        { typeof(RangePropertyViewModel), DataTemplateFactory.Create<RangeConfigurationPropertyView>() },
        { typeof(SecretPropertyViewModel), DataTemplateFactory.Create<SecretConfigurationPropertyView>() },
        { typeof(StringPropertyViewModel), DataTemplateFactory.Create<StringConfigurationPropertyView>() },
        { typeof(ColorPropertyViewModel), DataTemplateFactory.Create<ColorConfigurationPropertyView>() },
        { typeof(AbnormalityTrayPropertyViewModel), DataTemplateFactory.Create<AbnormalityTrayConfigurationPropertyView>() },
        { typeof(MonsterDetailsPropertyViewModel), DataTemplateFactory.Create<MonsterDetailsConfigurationPropertyView>() }
    };

    public static DataTemplate? FindBy(Type type)
    {
        bool foundTemplate = Templates.TryGetValue(type, out DataTemplate? dataTemplate);

        return foundTemplate ? dataTemplate : null;
    }

    public static void Register(Type type, DataTemplate dataTemplate)
    {
        Templates[type] = dataTemplate;
    }
}