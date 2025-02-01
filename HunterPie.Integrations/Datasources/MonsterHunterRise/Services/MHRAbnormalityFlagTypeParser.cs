using HunterPie.Core.Game.Data.Interfaces;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Services;
public class MHRAbnormalityFlagTypeParser : IAbnormalityFlagTypeParser
{
    public Enum? Parse(AbnormalityFlagType type, string value)
    {
        Type? enumType = type switch
        {
            AbnormalityFlagType.RiseCommon => typeof(CommonConditions),
            AbnormalityFlagType.RiseDebuff => typeof(DebuffConditions),
            AbnormalityFlagType.RiseAction => typeof(ActionFlags),
            _ => null
        };

        if (enumType is null)
            return null;

        return (Enum)Enum.Parse(enumType, value);
    }
}