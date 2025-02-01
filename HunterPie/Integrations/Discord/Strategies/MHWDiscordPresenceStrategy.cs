using DiscordRPC;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using System.Linq;

namespace HunterPie.Integrations.Discord.Strategies;

internal class MHWDiscordPresenceStrategy : IDiscordRichPresenceStrategy
{
    private readonly DiscordRichPresence _configuration;
    private readonly IScopedLocalizationRepository _localizationRepository;
    private readonly IContext _context;

    public MHWDiscordPresenceStrategy(
        DiscordRichPresence configuration,
        ILocalizationRepository localizationRepository,
        IContext context)
    {
        _configuration = configuration;
        _localizationRepository = localizationRepository.WithScope("//Strings/Client/Integrations/Discord");
        _context = context;
    }

    public string AppId => "567152028070051859";

    public void Update(RichPresence presence)
    {
        if (_context.Game.Player is not MHWPlayer player)
            return;

        string description = BuildDescription();
        string state = BuildState();
        string stageIdName = MHWContext.Strings.GetStageNameById(player.StageId);

        presence
            .WithDetails(description)
            .WithState(state)
            .WithAssets(new Assets
            {
                LargeImageKey = player.ZoneId != Stage.MainMenu
                    ? $"st{player.StageId}"
                    : "main-menu",
                LargeImageText = stageIdName,
                SmallImageKey = player.Weapon.Id != Weapon.None
                    ? $"weap{(int)player.Weapon.Id}"
                    : "hunter-rank",
                SmallImageText = _configuration.ShowCharacterInfo.Value switch
                {
                    true => _localizationRepository.FindStringBy("DRPC_RISE_CHARACTER_STRING_FORMAT")
                        .Replace("{Character}", _context.Game.Player.Name)
                        .Replace("{HighRank}", _context.Game.Player.HighRank.ToString())
                        .Replace("{MasterRank}", _context.Game.Player.MasterRank.ToString()),
                    _ => null
                }
            })
            .WithParty(new Party
            {
                ID = _context.Game.Player.Name,
                Max = _context.Game.Player.Party.MaxSize,
                Size = _context.Game.Player.Party.Size,
                Privacy = Party.PrivacySetting.Public
            });
    }

    private string BuildDescription()
    {
        string localizationStateId = (Stage)_context.Game.Player.StageId switch
        {
            Stage.MainMenu => "DRPC_STATE_MAIN_MENU",

            Stage.Astera
                or Stage.AsteraGatheringHub
                or Stage.ResearchBase
                or Stage.Seliana
                or Stage.SelianaGatheringHub
                or Stage.LivingQuarters
                or Stage.PrivateQuarters
                or Stage.PrivateSuite
                or Stage.SelianaRoom => "DRPC_STATE_IDLE",

            Stage.TrainingArea => "DRPC_WORLD_STATE_PRACTICE",

            _ => "DRPC_STATE_EXPLORING"
        };

        IMonster? targetMonster = _context.Game.Monsters.FirstOrDefault(it => it.Target == Target.Self);

        if (targetMonster is not { })
            return _localizationRepository.FindStringBy(localizationStateId);

        string descriptionString = _configuration.ShowMonsterHealth
            ? "DRPC_STATE_HUNTING"
            : "DRPC_STATE_HUNTING_NO_HEALTH";

        return _localizationRepository.FindStringBy(descriptionString)
            .Replace("{Monster}", targetMonster.Name)
            .Replace("{Percentage}", $"{targetMonster.Health / targetMonster.MaxHealth * 100:0}");
    }

    private string BuildState()
    {
        string localizationId = _context.Game.Player.Party.Size <= 1
            ? "DRPC_PARTY_STATE_SOLO_STRING"
            : "DRPC_PARTY_STATE_GROUP_STRING";

        return _localizationRepository.FindStringBy(localizationId);
    }
}