using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture.Test;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;

public class MockSpecializedToolViewModel : SpecializedToolViewModel
{
    private const double MAX_TIMER = 20;
    private const double MAX_COOLDOWN = 20;

    public MockSpecializedToolViewModel()
    {
        Id = SpecializedToolType.ChallengerMantle;
        MaxCooldown = MAX_COOLDOWN;
        Timer = MaxTimer = MAX_TIMER;

        MockBehavior.Run(() =>
        {

            if (IsRecharging)
            {
                Cooldown++;
                Timer = 120;
                IsRecharging = Cooldown != MaxCooldown;
            }
            else
            {
                Timer--;
                Cooldown = 0;
                IsRecharging = Timer <= 0;
            }
        });
    }
}