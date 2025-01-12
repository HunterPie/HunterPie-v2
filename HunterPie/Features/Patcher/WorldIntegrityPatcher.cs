using HunterPie.Core.Game;
using HunterPie.Core.Logger;

namespace HunterPie.Features.Patcher;

internal static class WorldIntegrityPatcher
{

    public static void Patch(IContext _)
    {
        Log.Warn("Make sure you have Stracker's Loader and it's dependencies installed.\nIf your game crash while using HunterPie, that means you don't have them installed.");
    }
}