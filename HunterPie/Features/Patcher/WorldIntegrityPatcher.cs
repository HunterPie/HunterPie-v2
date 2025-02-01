using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;

namespace HunterPie.Features.Patcher;

internal static class WorldIntegrityPatcher
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    public static void Patch(IContext _)
    {
        Logger.Warning("Make sure you have Stracker's Loader and it's dependencies installed.\nIf your game crash while using HunterPie, that means you don't have them installed.");
    }
}