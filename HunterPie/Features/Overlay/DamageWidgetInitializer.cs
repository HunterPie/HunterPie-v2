﻿using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Damage;

namespace HunterPie.Features.Overlay;

internal class DamageWidgetInitializer : IWidgetInitializer
{
    private IContextHandler _handler;

    public void Load(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

        if (!config.DamageMeterWidget.Initialize)
            return;

        _handler = new DamageMeterWidgetContextHandler(context);
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}