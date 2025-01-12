using DiscordRPC;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using System.Linq;

namespace HunterPie.Integrations.Discord;

internal sealed class WorldRichPresence : RichPresence
{
    private const string WorldAppId = "567152028070051859";
    private readonly MHWGame _game;

    protected override DiscordRichPresence Settings => ClientConfig.Config.World.RichPresence;
    public WorldRichPresence(MHWContext context) : base(WorldAppId, context.Game)
    {
        _game = (MHWGame)context.Game;
    }

    protected override void HandlePresence()
    {
        string description = null;

        description = (Stage)_game.Player.StageId switch
        {
            Stage.MainMenu => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_MAIN_MENU']"),

            Stage.Astera or Stage.AsteraGatheringHub or Stage.ResearchBase
            or Stage.Seliana or Stage.SelianaGatheringHub or Stage.LivingQuarters
            or Stage.PrivateQuarters or Stage.PrivateSuite or Stage.SelianaRoom => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_IDLE']"),

            Stage.TrainingArea => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_WORLD_STATE_PRACTICE']"),

            _ => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_EXPLORING']")
        };

        IMonster targetMonster = _game!.Monsters.FirstOrDefault(monster => monster.Target == Target.Self);
        if (targetMonster is not null)
        {
            string descriptionString = Settings.ShowMonsterHealth
                ? Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_HUNTING']")
                : Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_HUNTING_NO_HEALTH']");

            description = descriptionString
                .Replace("{Monster}", targetMonster.Name)
                .Replace("{Percentage}", $"{targetMonster.Health / targetMonster.MaxHealth * 100:0}");
        }

        var player = (MHWPlayer)_game.Player;

        string smallKeyText = Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_WORLD_CHARACTER_STRING_FORMAT']")
            .Replace("{Character}", _game.Player.Name)
            .Replace("{HighRank}", player.HighRank.ToString())
            .Replace("{MasterRank}", player.MasterRank.ToString());

        string state = _game.Player.Party.Size <= 1
            ? Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_PARTY_STATE_SOLO_STRING']")
            : Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_PARTY_STATE_GROUP_STRING']");

        _ = Presence.WithDetails(description)
            .WithAssets(new Assets()
            {
                LargeImageKey = player.ZoneId != Stage.MainMenu ? $"st{player.StageId}" : "main-menu",
                LargeImageText = MHWContext.Strings.GetStageNameById(player.StageId),
                SmallImageKey = player.Weapon.Id != Weapon.None ? $"weap{(int)player.Weapon.Id}" : "hunter-rank",
                SmallImageText = smallKeyText
            })
            .WithParty(new Party()
            {
                // TODO: Use session Id with party leader name hash as Id
                ID = _game.Player.Name ?? "",
                Max = _game.Player.Party.MaxSize,
                Size = _game.Player.Party.Size,
                Privacy = Party.PrivacySetting.Public
            })
            .WithState(state);
    }
}