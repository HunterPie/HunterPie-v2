#pragma once
#include <string>
#include "weapon.h"

namespace Core::Entities
{
    enum EntityType
    {
        PLAYER,
        NPC,
        PET,
        COMPANION,
        ENVIRONMENT,
        MONSTER,
        UNKNOWN
    };

    struct Entity
    {
        // Entity index in the HuntStatistics::entities array.
        int index;
        EntityType type;
    };

    struct EntityDamageData
    {
        // Pointer to the target Monster structure. (MHW/MHR)
        intptr_t target;
        Entity source;
        float rawDamage;
        float elementalDamage;
    };
}
