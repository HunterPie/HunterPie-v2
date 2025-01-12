using HunterPie.UI.Architecture.Test;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;

public class MockAbnormalityBarViewModel : AbnormalityBarViewModel
{
    private struct MockAbnormalityData
    {
        public string Name;
        public string Icon;
        public float MaxDuration;
        public bool IsDebuff;
        public bool IsBuildup;
    }
    private static readonly MockAbnormalityData[] MockAbnormalityDatas =
    {
        // Songs
        new() { Name = "ABNORMALITY_SELF_IMPROVEMENT", Icon = "ICON_SELFIMPROVEMENT", MaxDuration = 180f },
        new() { Name = "ABNORMALITY_EARPLUG_PLUS", Icon = "ICON_EARPLUGS+", MaxDuration = 200f },
        
        // Consumables
        new() { Name = "ABNORMALITY_MIGHT_SEED", Icon = "ITEM_MIGHT", MaxDuration = 250f },
        new() { Name = "ABNORMALITY_POWER_DRUM", Icon = "ICON_POWERDRUM", MaxDuration = 250f },
        new() { Name = "ABNORMALITY_IMMUNITY", Icon = "ICON_IMMUNITY", MaxDuration = 250f },

        // Foods
        new() { Name = "ABNORMALITY_DANGO_BOOSTER", Icon = "ICON_DANGOBOOSTER", MaxDuration = 540f },
        new() { Name = "ABNORMALITY_DANGO_GLUTTON", Icon = "ICON_DANGOGLUTTON", MaxDuration = 30f },

        // Debuffs
        new() { Name = "ABNORMALITY_POISON", Icon = "ICON_POISON", MaxDuration = 60, IsDebuff = true },
        new() { Name = "ABNORMALITY_HELLFIRE", Icon = "ICON_HELLFIRE", MaxDuration = 60, IsDebuff = true },
        new() { Name = "ABNORMALITY_THUNDER", Icon = "ELEMENT_THUNDER", MaxDuration = 45, IsDebuff = true },
        new() { Name = "ABNORMALITY_BUBBLES_PLUS", Icon = "ICON_BUBBLE+", MaxDuration = 60, IsDebuff = true },
        new() { Name = "ABNORMALITY_FIRE", Icon = "ELEMENT_FIRE", MaxDuration = 15, IsDebuff = true },
        new() { Name = "ABNORMALITY_STENCH", Icon = "ICON_STENCH", MaxDuration = 30, IsDebuff = true },
        new() { Name = "ABNORMALITY_RES_DOWN", Icon = "ICON_ALLRESDOWN", MaxDuration = 25, IsDebuff = true },

        // Buildup
        new() { Name = "ABNORMALITY_THE_FRENZY_BUILDUP", Icon = "ICON_THE_FRENZY", MaxDuration = 150, IsDebuff = true, IsBuildup = true },
    };

    public MockAbnormalityBarViewModel()
    {
        SetupAbnormalities();
        MockBehavior.Run(() =>
        {
            foreach (AbnormalityViewModel vm in Abnormalities)
            {
                vm.Timer -= 1;

                if (vm.Timer <= 0)
                    vm.Timer = vm.MaxTimer;
            }
        });
    }

    private void SetupAbnormalities()
    {
        foreach (MockAbnormalityData data in MockAbnormalityDatas)
        {
            var mock = new AbnormalityViewModel()
            {
                Icon = data.Icon,
                Name = data.Name,
                Timer = data.MaxDuration,
                MaxTimer = data.MaxDuration,
                IsBuff = !data.IsDebuff,
                IsBuildup = data.IsBuildup,
            };
            Abnormalities.Add(mock);
        }
    }
}