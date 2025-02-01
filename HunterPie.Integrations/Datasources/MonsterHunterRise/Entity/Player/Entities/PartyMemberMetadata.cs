using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Entities;

internal record PartyMemberMetadata(
    int Index,
    bool IsValid,
    MHRCharacterData Data
);