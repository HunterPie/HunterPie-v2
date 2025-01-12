using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Services;

public static class MHRAbnormalityAdapter
{
    public static MHRAbnormalityData Convert(AbnormalityDefinition schema, MHRAbnormalityStructure abnormality)
    {
        if (schema.IsInteger)
            return new MHRAbnormalityData { Timer = (float)abnormality.Timer };

        return schema.Category switch
        {
            "Consumables" => new MHRAbnormalityData { Timer = abnormality.Consumable.Timer },
            "Debuffs" => new MHRAbnormalityData { Timer = abnormality.Debuff.Timer },
            _ => new MHRAbnormalityData()
        };
    }
}