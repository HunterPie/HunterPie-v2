using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Settings.Converter.Internal;

namespace HunterPie.UI.Settings;

internal class SettingsModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<AbnormalityTrayConfigurationPropertyBuilder>()
            .WithSingle<BooleanConfigurationPropertyBuilder>()
            .WithSingle<ColorConfigurationPropertyBuilder>()
            .WithSingle<EnumConfigurationPropertyBuilder>()
            .WithSingle<FileSelectorConfigurationPropertyBuilder>()
            .WithSingle<KeybindingConfigurationPropertyBuilder>()
            .WithSingle<MonsterDetailsConfigurationPropertyBuilder>()
            .WithSingle<PositionConfigurationPropertyBuilder>()
            .WithSingle<RangeConfigurationPropertyBuilder>()
            .WithSingle<SecretConfigurationPropertyBuilder>()
            .WithSingle<StringConfigurationPropertyBuilder>()
            .WithSingle<ConfigurationAdapter>();
    }
}