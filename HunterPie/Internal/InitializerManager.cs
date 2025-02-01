using HunterPie.Core.Observability.Logging;
using HunterPie.DI;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Initializers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace HunterPie.Internal;

internal class InitializerManager
{
    private static readonly ILogger Logger = LoggerFactory.Create();
    private static readonly HashSet<IInitializer> CoreInitializers = new() { new MapperFactoryInitializer() };

    private static readonly Type[] Initializers =
    {
        typeof(FileStreamLoggerInitializer),
        typeof(CustomFontsInitializer),
        
        // Core
        typeof(CredentialVaultInitializer),
        typeof(LocalConfigInitializer),
        
        // Feature Flags
        typeof(FeatureFlagsInitializer),

        typeof(NativeLoggerInitializer),
        
        // Config
        typeof(RemoteConfigSyncInitializer),
        typeof(ClientConfigMigrationInitializer),
        typeof(ClientConfigInitializer),
        typeof(ConfigManagerInitializer),

        typeof(HunterPieLoggerInitializer),
        typeof(CustomThemeInitializer),

        typeof(ExceptionCatcherInitializer),
        typeof(DialogManagerInitializer),
        typeof(UITracerInitializer),
        typeof(ClientLocalizationInitializer),
        typeof(SystemTrayInitializer),
        typeof(ClientConfigBindingsInitializer),

        // GUI
        typeof(NavigationInitializer),
        typeof(NavigationTemplatesInitializer),
        typeof(AppNotificationsInitializer),
    };

    private static readonly Type[] UiInitializers =
    {
        typeof(HotkeyInitializer),

        // Debugging
        typeof(DebugWidgetInitializer)
    };

    public static async Task InitializeCore()
    {
        foreach (IInitializer initializer in CoreInitializers)
        {
            Logger.Debug($"Running {initializer.GetType().Name}");
            await initializer.Init();
        }
    }

    public static async Task InitializeAsync()
    {
        foreach (Type initializerType in Initializers)
        {
            if (DependencyContainer.Get(initializerType) is not IInitializer initializer)
                continue;

            Logger.Debug($"Running {initializer.GetType().Name}");
            await initializer.Init();
        }
    }

    public static void InitializeGUI()
    {
        // Make sure to run UI initializers in the main thread
        Application.Current.Dispatcher.Invoke(async () =>
        {
            foreach (Type initializerType in UiInitializers)
            {
                if (DependencyContainer.Get(initializerType) is not IInitializer initializer)
                    continue;

                await initializer.Init();
            }
        });
    }

    public static void Unload()
    {
        foreach (IInitializer initializer in CoreInitializers)
            if (initializer is IDisposable disposable)
                disposable.Dispose();

        foreach (Type initializerType in Initializers)
        {
            if (DependencyContainer.Get(initializerType) is not IDisposable disposable)
                continue;

            disposable.Dispose();
        }

        foreach (Type initializerType in UiInitializers)
            if (DependencyContainer.Get(initializerType) is IDisposable disposable)
                disposable.Dispose();
    }
}