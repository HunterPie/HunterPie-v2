#include "pch.h"
#include "RiseUtils.h"

#define MAX_SUNBREAK_BIG_MONSTER_ID 115
#define MIN_SUNBREAK_BIG_MONSTER_ID 76
#define MAX_BIG_MONSTER_ID 46
#define MIN_BIG_MONSTER_ID 0
//                             46    40         30         20         10          0
#define VANILLA_MONSTERS_MASK 0b1111111'1111111111'1111111111'1111111111'1111111111l

//                             115      106         96         86         76
#define SUNBREAK_MONSTERS_MASK 0b1111111110'0000000111'1111111111'1111111111l

bool Games::Rise::Utils::IsBigMonster(int32_t id)
{
    if (id >= MIN_SUNBREAK_BIG_MONSTER_ID && id <= MAX_SUNBREAK_BIG_MONSTER_ID)
        return (SUNBREAK_MONSTERS_MASK >> (id - MIN_SUNBREAK_BIG_MONSTER_ID)) & 1;
    else if (id >= MIN_BIG_MONSTER_ID && id <= MAX_BIG_MONSTER_ID)
        return (VANILLA_MONSTERS_MASK >> id) & 1;

    return false;
}