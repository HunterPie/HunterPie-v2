using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Player.Skills;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.Weapons;
public class MHWMeleeWeapon : CommonMeleeWeapon
{
    private int[]? _minimumSharpnessByLevel;
    protected readonly ISkillService _skillService;
    private int _weaponId;
    private Sharpness _sharpness = Sharpness.Red;
    private int _currentSharpness;
    private int _threshold;

    public override Weapon Id { get; }

    public override Sharpness Sharpness
    {
        get => _sharpness;
        protected set
        {
            if (value != _sharpness)
            {
                _sharpness = value;
                this.Dispatch(_onSharpnessLevelChange, new SharpnessEventArgs(this));
            }
        }
    }

    public override int CurrentSharpness
    {
        get => _currentSharpness;
        protected set
        {
            if (value != _currentSharpness)
            {
                _currentSharpness = value;
                this.Dispatch(_onSharpnessChange, new SharpnessEventArgs(this));
            }
        }
    }

    public override int[]? SharpnessThresholds { get; protected set; }

    public override int MaxSharpness { get; protected set; }

    public override int Threshold
    {
        get => _threshold;
        protected set
        {
            if (value == _threshold)
                return;

            _threshold = value;
            this.Dispatch(_onSharpnessChange, new SharpnessEventArgs(this));
        }
    }

    public MHWMeleeWeapon(
        IGameProcess process,
        ISkillService skillService,
        Weapon id) : base(process)
    {
        _skillService = skillService;
        Id = id;
    }

    [ScannableMethod]
    protected async Task GetWeaponSharpness()
    {
        _minimumSharpnessByLevel ??= await Memory.ReadAsync<int>(
            address: AddressMap.GetAbsolute("MINIMUM_SHARPNESSES_ADDRESS"),
            count: 8
        );

        nint weaponDataPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("WEAPON_DATA_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_DATA_OFFSETS")
        );

        if (weaponDataPtr.IsNullPointer())
            return;

        int weaponId = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_ID_OFFSETS")
        );

        MHWSharpnessStructure sharpness = await Memory.DerefAsync<MHWSharpnessStructure>(
            address: AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            offsets: AddressMap.Get<int[]>("WEAPON_SHARPNESS_OFFSETS")
        );

        if (sharpness.Level is >= Sharpness.Invalid or < Sharpness.Red)
            return;

        if (SharpnessThresholds is null || _weaponId != weaponId)
        {
            nint weaponSharpnessArrayPtr = await Memory.ReadAsync(
                address: weaponDataPtr,
                offsets: new[] { weaponId * sizeof(long), 0x0C }
            );
            short[] sharpnesses = await Memory.ReadAsync<short>(weaponSharpnessArrayPtr, 7);

            SharpnessThresholds = sharpnesses.Select(t => (int)t)
                                             .ToArray();

            _weaponId = weaponId;
        }

        Sharpness currentLevel = SharpnessThresholds.GetCurrentSharpness(sharpness.Sharpness);

        if (currentLevel is >= Sharpness.Invalid or <= Sharpness.Broken)
            return;

        Skill handicraft = _skillService.GetSkillBy(54);
        int maxHits = SharpnessThresholds.MaximumSharpness(_minimumSharpnessByLevel, currentLevel, sharpness.MaxLevel, handicraft);

        Sharpness = currentLevel;
        MaxSharpness = maxHits;
        Threshold = CalculateCurrentThreshold(currentLevel, SharpnessThresholds);
        CurrentSharpness = sharpness.Sharpness;
    }
}