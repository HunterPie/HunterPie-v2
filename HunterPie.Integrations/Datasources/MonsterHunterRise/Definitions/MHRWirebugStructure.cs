namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

public struct MHRWirebugStructure
{
    public long Ref; // 0
    public int Unk0; // 80
    public int Unk1; // 8 + 4 
    public float Cooldown;
    public float MaxCooldown;
    public float ExtraCooldown;
}

public struct MHRWirebugExtrasStructure
{
    public float Timer;
}