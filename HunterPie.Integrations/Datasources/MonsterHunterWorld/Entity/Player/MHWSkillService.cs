using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Game.Entity.Player.Skills;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;

public class MHWSkillService : Scannable, ISkillService, IDisposable
{
    private readonly Skill[] _skills = Enumerable.Repeat(false, 226)
        .Select(_ => new Skill())
        .ToArray();

    public MHWSkillService(IGameProcess process) : base(process)
    {
        ScanManager.Add(this);
    }

    public Skill GetSkillBy(int id)
    {
        return _skills.ElementAtOrDefault(id);
    }

    [ScannableMethod]
    private async Task GetGearSkills()
    {
        nint gearSkillsPtr = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            offsets: AddressMap.Get<int[]>("GEAR_SKILL_OFFSETS")
        );

        MHWGearSkill[] skills = await Memory.ReadAsync<MHWGearSkill>(gearSkillsPtr, 226);

        for (int i = 0; i < skills.Length; i++)
            _skills[i].Level = skills[i].LevelGear;
    }

    public void Dispose()
    {
        ScanManager.Remove(this);
    }
}