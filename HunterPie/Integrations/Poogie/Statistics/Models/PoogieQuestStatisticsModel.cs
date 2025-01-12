using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieQuestStatisticsModel(
    [property: JsonProperty("game_type")] GameType GameType,
    [property: JsonProperty("started_at")] DateTime StartedAt,
    [property: JsonProperty("finished_at")] DateTime FinishedAt,
    [property: JsonProperty("uploaded_at")] DateTime UploadedAt,
    [property: JsonProperty("quest")] PoogieQuestDetailsModel? Quest,
    [property: JsonProperty("players")] PoogiePlayerStatisticsModel[] Players,
    [property: JsonProperty("monsters")] PoogieMonsterStatisticsModel[] Monsters,
    [property: JsonProperty("hash")] string Hash
)
{
    public HuntStatisticsModel ToEntity() =>
        new HuntStatisticsModel(
            Game: GameType.ToEntity(),
            StartedAt: StartedAt,
            FinishedAt: FinishedAt,
            UploadedAt: UploadedAt,
            Quest: Quest?.ToEntity(),
            Players: Players.Select(it => it.ToEntity()).ToList(),
            Monsters: Monsters.Select(it => it.ToEntity()).ToList(),
            Hash: Hash
        );

    public static PoogieQuestStatisticsModel From(HuntStatisticsModel model) =>
        new PoogieQuestStatisticsModel(
            GameType: model.Game.ToApiModel(),
            StartedAt: model.StartedAt,
            FinishedAt: model.FinishedAt,
            UploadedAt: model.UploadedAt,
            Quest: model.Quest is null ? null : PoogieQuestDetailsModel.From(model.Quest),
            Players: model.Players.Select(PoogiePlayerStatisticsModel.From).ToArray(),
            Monsters: model.Monsters.Select(PoogieMonsterStatisticsModel.From).ToArray(),
            Hash: model.Hash
        );
}