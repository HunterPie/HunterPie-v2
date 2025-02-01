using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Services;
public interface IAbnormalityCategorizationService
{
    AbnormalityCategory Categorize(IAbnormality abnormality);
}