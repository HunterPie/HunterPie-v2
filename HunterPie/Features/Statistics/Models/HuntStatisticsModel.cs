using HunterPie.Core.Client.Configuration.Enums;
using System;
using System.Collections.Generic;

namespace HunterPie.Features.Statistics.Models;

internal record HuntStatisticsModel(
    GameType Game,
    QuestDetailsModel? Quest,
    List<PartyMemberModel> Players,
    List<MonsterModel> Monsters,
    DateTime StartedAt,
    DateTime FinishedAt,
    DateTime UploadedAt,
    string Hash
);