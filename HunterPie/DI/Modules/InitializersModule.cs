using HunterPie.DI.Module;
using HunterPie.Internal.Initializers;

namespace HunterPie.DI.Modules;

public class InitializersModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<FileStreamLoggerInitializer>()
            .WithSingle<CustomFontsInitializer>()
            .WithSingle<CredentialVaultInitializer>()
            .WithSingle<LocalConfigInitializer>()
            .WithSingle<FeatureFlagsInitializer>()
            .WithSingle<NativeLoggerInitializer>()
            .WithSingle<RemoteConfigSyncInitializer>()
            .WithSingle<ClientConfigMigrationInitializer>()
            .WithSingle<ClientConfigInitializer>()
            .WithSingle<ConfigManagerInitializer>()
            .WithSingle<HunterPieLoggerInitializer>()
            .WithSingle<CustomThemeInitializer>()
            .WithSingle<ExceptionCatcherInitializer>()
            .WithSingle<DialogManagerInitializer>()
            .WithSingle<UITracerInitializer>()
            .WithSingle<ClientLocalizationInitializer>()
            .WithSingle<SystemTrayInitializer>()
            .WithSingle<ClientConfigBindingsInitializer>()
            .WithSingle<NavigationInitializer>()
            .WithSingle<NavigationTemplatesInitializer>()
            .WithSingle<AppNotificationsInitializer>()
            .WithSingle<HotkeyInitializer>()
            .WithSingle<DebugWidgetInitializer>();
    }
}