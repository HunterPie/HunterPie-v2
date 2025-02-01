using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Data.Interfaces;

#nullable enable
public interface IAbnormalityFlagTypeParser
{
    public Enum? Parse(AbnormalityFlagType type, string value);
}