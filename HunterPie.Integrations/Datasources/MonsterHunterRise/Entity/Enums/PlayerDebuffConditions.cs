﻿namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;

[Flags]
public enum DebuffConditions : ulong
{
    None = 0,
    NoData1 = 1,
    FireL = 1 << 1,
    NoData2 = 1 << 2,
    WaterL = 1 << 3,
    NoData3 = 1 << 4,
    ThunderL = 1 << 5,
    NoData4 = 1 << 6,
    IceL = 1 << 7,
    NoData5 = 1 << 8,
    DragonL = 1 << 9,
    AllResDownS = 1 << 10,
    AllResDownL = 1 << 11,
    VitalDamaging = 1 << 12,
    Bubble = 1 << 13,
    RedBubble = 1 << 14,
    GreenBubble = 1 << 15,
    VirusLatency = 1 << 16,
    VirusOnsetNearness = 1 << 17,
    VirusOvercomeNearness = 1 << 18,
    VirusOnset = 1 << 19,
    VirusOvercome = 1 << 20,
    Territory = 1 << 21,
    OniBomb1 = 1 << 22,
    OniBomb2 = 1 << 23,
    Bomb1 = 1 << 24,
    Bomb2 = 1 << 25,
    Bomb3 = 1 << 26,
    GoldMudSlip = 1 << 27,
    MagmaSlip = 1 << 28,
    EnemyFireSlip = 1 << 29,
    NoData6 = 1 << 30,
    NoData7 = 1UL << 31,
    Poison = 1UL << 32,
    NoxiousPoison = 1UL << 33,
    DeadlyPoison = 1UL << 34,
    Sleep = 1UL << 35,
    Paralyze = 1UL << 36,
    Stun = 1UL << 37,
    Stink = 1UL << 38,
    LeadEnemy = 1UL << 39,
    LeadEnemyAfter = 1UL << 40,
    BubbleDaruma = 1UL << 41,
    RedBubbleDaruma = 1UL << 42,
    GreenBubbleDaruma = 1UL << 43,
    Blooding = 1UL << 44,
    Bleeding = 1UL << 45,
    Confusion = 1UL << 46,
    DefenceDownS = 1UL << 47,
    DefenceDownL = 1UL << 48,
    Beto = 1UL << 49,
    VirusLatencyMiddle = 1UL << 51,
    MysteryDebuff = 1UL << 52,
    MysteryDebuffHeal = 1UL << 53
}