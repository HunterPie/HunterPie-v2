using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;
public class MHRMeleeWeapon : CommonMeleeWeapon
{
    private long _weaponSharpnessPointer;
    private Sharpness _sharpness;
    private int _currentSharpness;

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

    public override int Threshold { get; protected set; }

    public MHRMeleeWeapon(
        IGameProcess process,
        IScanService scanService,
        Weapon id) : base(process, scanService)
    {
        Id = id;
    }

    [ScannableMethod]
    protected async Task GetWeaponSharpness()
    {
        nint sharpnessArrayPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("SHARPNESS_ADDRESS"),
            offsets: AddressMap.Get<int[]>("SHARPNESS_ARRAY_OFFSETS")
        );

        if (sharpnessArrayPtr.IsNullPointer())
            return;

        MHRSharpnessStructure structure = await Memory.DerefAsync<MHRSharpnessStructure>(
            address: AddressMap.GetAbsolute("SHARPNESS_ADDRESS"),
            offsets: AddressMap.Get<int[]>("SHARPNESS_OFFSETS")
        );

        if (structure.Level is >= Sharpness.Invalid or < Sharpness.Red)
            return;

        if (SharpnessThresholds is null || _weaponSharpnessPointer != sharpnessArrayPtr)
        {
            int[] sharpnessValues = await Memory.ReadArrayAsync<int>(sharpnessArrayPtr);
            SharpnessThresholds = CalculateThresholds(sharpnessValues);
            _weaponSharpnessPointer = sharpnessArrayPtr;
        }

        Sharpness = structure.Level;
        MaxSharpness = CalculateMaxThreshold(structure.Level, SharpnessThresholds, structure.MaxHits);
        Threshold = CalculateCurrentThreshold(structure.Level, SharpnessThresholds);
        CurrentSharpness = structure.Hits;
    }

    private static int[] CalculateThresholds(IReadOnlyList<int> sharpnesses)
    {
        int[] thresholds = new int[sharpnesses.Count];

        int sum = 0;
        for (int i = 0; i < sharpnesses.Count; i++)
        {
            if (sharpnesses[i] == 0)
                continue;

            sum += sharpnesses[i];
            thresholds[i] = sum;
        }

        return thresholds;
    }


}