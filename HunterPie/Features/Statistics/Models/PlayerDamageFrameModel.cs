using System;

namespace HunterPie.Features.Statistics.Models;

internal record PlayerDamageFrameModel(
    float Damage,
    DateTime DealtAt
);