using HunterPie.Core.Settings.Types;
using HunterPie.DI;
using HunterPie.UI.Settings.Converter;
using HunterPie.UI.Settings.Converter.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Range = HunterPie.Core.Settings.Types.Range;

namespace HunterPie.UI.Settings;

public static class ConfigurationPropertyProvider
{
    private static readonly Dictionary<Type, Type> Builders = new()
    {
        { typeof(bool), typeof(BooleanConfigurationPropertyBuilder) },
        { typeof(Color), typeof(ColorConfigurationPropertyBuilder) },
        { typeof(Enum), typeof(EnumConfigurationPropertyBuilder) },
        { typeof(IFileSelector), typeof(FileSelectorConfigurationPropertyBuilder) },
        { typeof(Keybinding), typeof(KeybindingConfigurationPropertyBuilder) },
        { typeof(Position), typeof(PositionConfigurationPropertyBuilder) },
        { typeof(Range), typeof(RangeConfigurationPropertyBuilder) },
        { typeof(Secret), typeof(SecretConfigurationPropertyBuilder) },
        { typeof(string), typeof(StringConfigurationPropertyBuilder) },
        { typeof(AbnormalityTrays), typeof(AbnormalityTrayConfigurationPropertyBuilder) },
        { typeof(MonsterDetailsConfiguration), typeof(MonsterDetailsConfigurationPropertyBuilder) }
    };

    public static IConfigurationPropertyBuilder? FindBy(Type type)
    {
        Type? interfaceType = type.GetInterfaces()
            .FirstOrDefault(Builders.ContainsKey);

        if (interfaceType is { })
            return DependencyContainer.Get(Builders[interfaceType]) as IConfigurationPropertyBuilder;

        Type? innerType = type;

        if (type.IsGenericType)
            innerType = type.GenericTypeArguments?.FirstOrDefault();

        Type? builderType = innerType switch
        {
            null => null,
            { IsEnum: true } => Builders.GetValueOrDefault(typeof(Enum)),
            _ => Builders.GetValueOrDefault(innerType)
        };

        if (builderType is not { })
            return null;

        return DependencyContainer.Get(builderType) as IConfigurationPropertyBuilder;
    }

    public static void RegisterBuilder(Type type, Type builderType)
    {
        Builders.Add(type, builderType);
    }
}