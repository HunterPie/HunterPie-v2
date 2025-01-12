using HunterPie.UI.Architecture.Test;
using System;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;

public class MockWirebugsViewModel : WirebugsViewModel
{
    public MockWirebugsViewModel()
    {
        IsAvailable = true;
        Elements.Add(
            new()
            {
                Cooldown = 5,
                MaxCooldown = 12,
                OnCooldown = true,
                IsAvailable = true
            }
        );
        Elements.Add(
            new()
            {
                Cooldown = 0,
                MaxCooldown = 12,
                OnCooldown = false,
                IsAvailable = true
            }
        );
        Elements.Add(
            new()
            {
                Cooldown = 5,
                MaxCooldown = 12,
                Timer = 200,
                MaxTimer = 300,
                OnCooldown = true,
                IsAvailable = true,
                IsTemporary = true,
            }
        );
        MockBehavior.Run(() =>
        {
            foreach (WirebugViewModel vm in Elements)
            {
                vm.Timer -= Math.Min(vm.MaxTimer, 0.01);

                if (vm.Timer <= 0)
                    vm.Timer = vm.MaxTimer;

                if (!vm.OnCooldown)
                    continue;

                vm.Cooldown -= 0.01;

                if (vm.Cooldown <= 0)
                {
                    vm.Cooldown = 0;
                    vm.OnCooldown = false;
                }
            }
        }, 0.01f);
    }
}