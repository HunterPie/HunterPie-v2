using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Features.Statistics.Models;

internal record MonsterModel(
    int Id,
    float MaxHealth,
    Crown Crown,
    MonsterStatusModel Enrage,
    DateTime? HuntStartedAt,
    DateTime? HuntFinishedAt,
    MonsterHuntType? HuntType
);