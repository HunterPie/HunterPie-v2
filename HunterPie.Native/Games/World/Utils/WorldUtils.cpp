#include "pch.h";
#include "WorldUtils.h"

namespace Games::World::Utils {

    // Each bit indicates whether a monsterId represents a large monster.
    // Also tracks the large pillar in the training area (59)
    //                                      63 60         50         40         30         20         10          0
    const uint64_t BIG_MONSTER_BITSET_LO = 0b1110'1000000010'0000000000'1111111111'1111111111'1111111111'1010010011;
    //                                                                  100         90         80         70     64
    const uint64_t BIG_MONSTER_BITSET_HI = 0b00000000'0000000000'0000000011'1111111111'1110000011'1111111111'111111;

    bool IsLargeMonster(int32_t monsterId)
    {
        if (monsterId < 64) return (BIG_MONSTER_BITSET_LO >> monsterId) & 1;
        if (monsterId < 128) return (BIG_MONSTER_BITSET_HI >> (monsterId - 64)) & 1;
        return false;
    }

}
