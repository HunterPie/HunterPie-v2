using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Services;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Services;

public class MHRAbnormalityCategorizationService : IAbnormalityCategorizationService
{
    public AbnormalityCategory Categorize(IAbnormality abnormality)
    {
        return (abnormality.Id) switch
        {
            "ABN_NATURAL_HEALING" or "ABN_GOURMET_FISH" => AbnormalityCategory.NaturalHealing,
            "ABN_POISON" or "ABN_VENOM" => AbnormalityCategory.Poison,
            "ABN_FIRE" => AbnormalityCategory.Fire,
            "ABN_WATER" => AbnormalityCategory.Water,
            "ABN_ICE" => AbnormalityCategory.Ice,
            "ABN_BLEED" => AbnormalityCategory.Bleed,
            _ => AbnormalityCategory.None
        };
    }
}