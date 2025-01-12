using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MHRPlayerHudStructure
{
    public float Health;
    public float RecoverableHealth;
    public float MaxHealth;
    public float CurrentHealth; // Seems to be the same as Health?
    public float MaximumHealth; // Same as MaxHealth?
    public long Unk;
    public float Heal;
    public long Unk1;
    public float Stamina;
    public float MaxStamina;
    public float MaxExtendableStamina;
}