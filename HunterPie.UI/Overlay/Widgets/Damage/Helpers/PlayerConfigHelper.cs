using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Types;
using System;

namespace HunterPie.UI.Overlay.Widgets.Damage.Helpers;

public static class PlayerConfigHelper
{
    public static Color GetColorFromPlayer(GameProcessType game, int slot, bool isSelf = false)
    {
        DamageMeterWidgetConfig config = ClientConfigHelper.GetOverlayConfigFrom(game).DamageMeterWidget;

        return isSelf
            ? config.PlayerSelf
            : slot switch
            {
                0 => config.PlayerFirst,
                1 => config.PlayerSecond,
                2 => config.PlayerThird,
                3 => config.PlayerFourth,
                4 or 5 => config.NpcColor,
                _ => throw new Exception("invalid player slot")
            };
    }
}