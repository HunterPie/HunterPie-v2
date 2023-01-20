﻿namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;

[Flags]
public enum CommonConditions : ulong
{
    None = 0,
    AttackUp = 1,
    DefenceUp = 1 << 1,
    StaminaUp = 1 << 2,
    Immunity = 1 << 3,
    CriticalUp = 1 << 4,
    SuperArmor = 1 << 5,
    ElementAttackUp = 1 << 6,
    Stamina = 1 << 7,
    HpRegene = 1 << 8,
    HpFish = 1 << 9,
    HyperArmor = 1 << 10,
    DamageDown = 1 << 11,
    EarS = 1 << 12,
    EscapeUp = 1 << 13,
    HealthRegen = 1 << 14,
    AttackUpEffectOnly = 1 << 15,
    Reverse16 = 1 << 16,
    Reverse17 = 1 << 17,
    Reverse18 = 1 << 18,
    Reverse19 = 1 << 19,
    Reverse20 = 1 << 20,
    Reverse21 = 1 << 21,
    Reverse22 = 1 << 22,
    Reverse23 = 1 << 23,
    Reverse24 = 1 << 24,
    Reverse25 = 1 << 25,
    Heroics = 1 << 26,
    PeakPerformance = 1 << 27,
    Dragonheart = 1 << 28,
    LatentPower = 1 << 29,
    Agitator = 1 << 30,
    OffensiveGuard = 1UL << 31,
    Defiance = 1UL << 32,
    BladescaleHone = 1UL << 33,
    RubyWirebug = 1UL << 34,
    GoldWirebug = 1UL << 36,
    Redirection = 1UL << 40,
    SymbiosisLevel1 = 1UL << 41,
    SymbiosisLevel2 = 1UL << 42,
    SymbiosisLevel3 = 1UL << 43,
    FuriousHalf = 1UL << 44,
    FuriousFull = 1UL << 45,
    DangoDefender = 1UL << 46,
    DebuffAttackUp = 1UL << 47,
    EarL = 1UL << 48,
    TremorNegated = 1UL << 49,
    WindPressureNegated = 1UL << 50,
    VirusOvercomeCriticalUp = 1UL << 51,
    BloodySkillHeal = 1UL << 52,
    FuriousStaminaBuff = 1UL << 53,
    InterpidHeart = 1UL << 54,
    WireStopRegene = 1UL << 55,
    VirusOvercomeIcon = 1UL << 56,
    WindMantle = 1UL << 57,
    BerserkRed = 1UL << 58,
    BerserkBlue = 1UL << 59,
    StrifeSmall = 1UL << 60,
    StrifeLarge = 1UL << 61,
    PowderMantle = 1UL << 62
}