using DiscordRPC;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Domain.Utils;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using System;
using System.Linq;

namespace HunterPie.Integrations.Discord.Strategies;

internal class MHRDiscordPresenceStrategy : IDiscordRichPresenceStrategy
{
    private readonly DiscordRichPresence _configuration;
    private readonly IScopedLocalizationRepository _localizationRepository;
    private readonly IContext _context;

    public MHRDiscordPresenceStrategy(
        DiscordRichPresence configuration,
        ILocalizationRepository localizationRepository,
        IContext context)
    {
        _configuration = configuration;
        _localizationRepository = localizationRepository.WithScope("//Strings/Client/Integrations/Discord");
        _context = context;
    }

    public string AppId => "932399108017242182";

    public void Update(RichPresence presence)
    {
        string description = BuildDescription();
        string state = BuildState();
        string stageIdName = MHRContext.Strings.GetStageNameById(_context.Game.Player.StageId);
        bool isUnmappedStage = stageIdName.StartsWith("Unknown");

        presence
            .WithDetails(description)
            .WithState(state)
            .WithAssets(new Assets
            {
                LargeImageKey = isUnmappedStage switch
                {
                    true => "unknown",
                    _ => _context.Game.Player.StageId.ToImageKey()
                },
                LargeImageText = stageIdName,
                SmallImageKey = _context.Game.Player.Weapon.Id switch
                {
                    Weapon.None => null,
                    _ => Enum.GetName(typeof(Weapon), _context.Game.Player.Weapon.Id)?.ToLower() ?? "unknown"
                },
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
        string localizationStateId = _context.Game.Player.StageId switch
        {
            -2 => "DRPC_STATE_LOADING",
            -1 => "DRPC_STATE_MAIN_MENU",
            5 => "DRPC_RISE_STATE_PRACTICE",
            >= 0 and < 199 => "DRPC_STATE_IDLE",
            207 => "DRPC_RISE_STATE_RAMPAGE",
            199 => "DRPC_RISE_STATE_CHAR_SELECTION",
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