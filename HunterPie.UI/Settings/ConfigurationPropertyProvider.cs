using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
using HunterPie.UI.Settings.Converter.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Range = HunterPie.Core.Settings.Types.Range;

namespace HunterPie.UI.Settings;

#nullable enable
public static class ConfigurationPropertyProvider
{
    private static readonly Dictionary<Type, IConfigurationPropertyBuilder> Builders = new()
    {
        { typeof(bool), new BooleanConfigurationPropertyBuilder() },
        { typeof(Color), new ColorConfigurationPropertyBuilder() },
        { typeof(Enum), new EnumConfigurationPropertyBuilder() },
        { typeof(IFileSelector), new FileSelectorConfigurationPropertyBuilder() },
        { typeof(Keybinding), new KeybindingConfigurationPropertyBuilder() },
        { typeof(Position), new PositionConfigurationPropertyBuilder() },
        { typeof(Range), new RangeConfigurationPropertyBuilder() },
        { typeof(Secret), new SecretConfigurationPropertyBuilder() },
        { typeof(string), new StringConfigurationPropertyBuilder() },
        { typeof(AbnormalityTrays), new AbnormalityTrayConfigurationPropertyBuilder() },
        { typeof(MonsterDetailsConfiguration), new MonsterDetailsConfigurationPropertyBuilder() }
    };

    public static IConfigurationPropertyBuilder? FindBy(Type type)
    {
        Type? interfaceType = type.GetInterfaces()
            .FirstOrDefault(it => Builders.ContainsKey(it));

        if (interfaceType is { })
            return Builders[interfaceType];

        Type? innerType = type;

        if (type.IsGenericType)
            innerType = type.GenericTypeArguments?.FirstOrDefault();

        return innerType switch
        {
            null => null,
            { IsEnum: true } => FindBy(typeof(Enum)),
            _ => Builders.GetValueOrDefault(innerType)
        };
    }

    public static void RegisterBuilder(Type type, IConfigurationPropertyBuilder builder)
    {
        Builders.Add(type, builder);
    }
}