using HunterPie.Core.Client.Configuration.Enums;
using System;
using System.Diagnostics;

namespace HunterPie.Internal;

internal static class Steam
{
    public const string MONSTER_HUNTER_WORLD_APP_ID = "582010";
    public const string MONSTER_HUNTER_RISE_APP_ID = "1446780";

    public static void RunGameBy(GameType type)
    {
        string appId = type switch
        {
            GameType.Rise => MONSTER_HUNTER_RISE_APP_ID,
            GameType.World => MONSTER_HUNTER_WORLD_APP_ID,
            _ => throw new NotImplementedException(),
        };

        Process.Start(new ProcessStartInfo()
        {
            FileName = $"steam://run/{appId}",
            UseShellExecute = true
        });
    }
}