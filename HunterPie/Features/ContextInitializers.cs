using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Backup.Services;
using HunterPie.Features.Statistics.Services;
using HunterPie.Game.Rise;
using HunterPie.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features;

internal static class ContextInitializers
{
    private static readonly List<IContextInitializer> Instances = new();

    private static readonly Type[] Initializers =
    {
        typeof(MHWContextInitializer),
        typeof(MHRContextInitializer),
        typeof(GameSaveBackupService),
        typeof(QuestTrackerService)
    };

    public static Task InitializeAsync(Context context)
    {
        if (Instances is not { Count: 0 })
            throw new Exception("Context has already been initialized");

        Initializers.Select(DependencyContainer.Get)
            .TryCast<IContextInitializer>()
            .ForEach(Instances.Add);

        Task[] tasks = Instances.Select(it => it.InitializeAsync(context))
            .ToArray();

        Task.WaitAll(tasks);

        return Task.CompletedTask;
    }

    public static void Dispose()
    {
        Instances
            .TryCast<IDisposable>()
            .DisposeAll();

        Instances.Clear();
    }
}