using HunterPie.Core.Game.Data.Definitions;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.Data.Repository;

public static class MonsterPartRepository
{
    private static readonly Dictionary<PartGroupType, string[]> GroupAssociations = new()
    {
        {
            PartGroupType.Head,
            new[]
            {
                "PART_HEAD", "PART_HEAD_MUD", "PART_SILVER_SPIKES_HEAD", "PART_JAW", "PART_GLOWING_HEAD",
                "PART_EMERGE_SNOW_HEAD", "PART_HEAD_SNOW", "PART_HEAD_ICE", "PART_HEAD_ROCK", "PART_MANE",
            }
        },
        { PartGroupType.Horn, new[] { "PART_HORNS", "PART_HORN", "PART_HORNS_2", "PART_HORNS_GOLD", "PART_ANTLERS", } },
        { PartGroupType.Neck, new[] { "PART_NECK", "PART_THROAT", "PART_L_NECK_ROCK", "PART_R_NECK_ROCK", } },
        {
            PartGroupType.Body,
            new[]
            {
                "PART_TORSO", "PART_BACK", "PART_TORSO_MUD", "PART_BODY", "PART_UPPER_BODY", "PART_REAR",
                "PART_LOWER_BODY", "PART_UPPER_BACK", "PART_LOWER_BACK", "PART_DORSAL_FIN", "PART_ABDOMEN",
                "PART_CHEST", "PART_LOWER_TORSO", "PART_CHEST_WINDSAC", "PART_BACK_WINDSAC", "PART_FIN",
                "PART_L_CHEST_GOLD", "PART_R_CHEST_GOLD", "PART_EMERGE_SNOW_BODY", "PART_BODY_SNOW",
                "PART_BODY_ICE", "PART_CHEST_ROCK", "PART_BODY_LEGS",
            }
        },
        {
            PartGroupType.Wings,
            new[]
            {
                "PART_L_WING", "PART_R_WING", "PART_WINGS", "PART_WING_CLAW", "PART_SILVER_SPIKES_L_WING",
                "PART_SILVER_SPIKES_R_WING", "PART_WINGS_ICE", "PART_L_WING_ROCK", "PART_R_WING_ROCK",
            }
        },
        {
            PartGroupType.Arms,
            new[]
            {
                "PART_L_ARM", "PART_R_ARM", "PART_FORELEGS", "PART_L_CUTWING", "PART_R_CUTWING", "PART_ARMS",
                "PART_ARMS_MUD", "PART_CLAW", "PART_R_CLAW", "PART_L_CLAW", "PART_L_FORELEG", "PART_R_FORELEG",
                "PART_L_ARM_ICE", "PART_R_ARM_ICE", "PART_FORELEG", "PART_L_LIMBS", "PART_R_LIMBS", "PART_LIMBS",
                "PART_SILVER_SPIKES_L_ARM", "PART_SILVER_SPIKES_R_ARM", "PART_L_ARM_GOLD", "PART_R_ARM_GOLD",
                "PART_ARMS_ICE", "PART_L_ARM_ROCK", "PART_R_ARM_ROCK",
            }
        },
        {
            PartGroupType.Legs,
            new[]
            {
                "PART_L_LEG", "PART_R_LEG", "PART_H_LEGS", "PART_L_LEG_MUD", "PART_R_LEG_MUD", "PART_L_LEGS",
                "PART_R_LEGS", "PART_LEGS", "PART_L_H_LEG", "PART_R_H_LEG", "PART_B_LEG_FINS", "PART_H_LEG",
                "PART_L_LEG_GOLD", "PART_R_LEG_GOLD",
            }
        },
        {
            PartGroupType.Tail,
            new[]
            {
                "PART_TAIL", "PART_TAIL_MUD", "PART_TAIL_TIP", "PART_TAIL_WINDSAC", "PART_INFLATED_TAIL",
                "PART_L_TAIL_GOLD", "PART_R_TAIL_GOLD", "PART_GLOWING_TAIL", "PART_EMERGE_SNOW_TAIL",
                "PART_TAIL_SNOW", "PART_TAIL_ROCK",
            }
        },
        {
            PartGroupType.Special,
            new[]
            {
                "PART_RAGE", "PART_EXPLOSION_WEAKENING", "PART_COUNTERATTACK", "PART_KNOCKDOWN", "PART_BIG_FLINCH",
                "PART_QURIO_THRESHOLD", "PART_CHARGE", "PART_EXHAUST_ORGAN_CENTRAL", "PART_EXHAUST_ORGAN_HEAD",
                "PART_EXHAUST_ORGAN_CRATER", "PART_EXHAUST_ORGAN_REAR", "PART_REPEL", "PART_SKY_FALL",
                "PART_BALLOON", "PART_EMISSIONS", "PART_MANE_GOLD",
            }
        },
        {
            PartGroupType.Misc,
            new[]
            {
                "PART_UNKNOWN", "PART_TO_BE_MAPPED", "PART_SPONGE", "PART_ROLLING", "PART_SHELL", "PART_MUD_BALL",
                "PART_THUNDERBALLS", "PART_ROCK", "PART_POT", "PART_ANTENNA", "PART_CREST", "PART_WEAK_L_SHELL",
                "PART_WEAK_R_SHELL", "PART_L_BONE", "PART_R_BONE",
            }
        }
    };

    private static readonly Dictionary<string, PartGroupType> PartNameAssociations = GroupAssociations
        .SelectMany(it => it.Value.Select(value => new { Group = it.Key, Key = value }))
        .ToDictionary(it => it.Key, it => it.Group);

    /// <summary>
    /// Gets group type based on part name
    /// </summary>
    /// <param name="partName">Part name</param>
    /// <returns>Group type</returns>
    public static PartGroupType? FindBy(string partName)
    {
        return PartNameAssociations.GetValueOrDefault(partName);
    }
}