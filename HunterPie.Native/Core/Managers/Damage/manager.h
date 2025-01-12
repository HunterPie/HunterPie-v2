#pragma once
#include <unordered_map>
#include "Core/Entities/damage.h"

using namespace Core::Entities;

namespace HunterPie::Core::Damage
{
    struct HuntStatistics
    {
        EntityDamageData entities[10];
    };

    class DamageTrackManager
    {
    private:
        std::unordered_map<intptr_t, HuntStatistics*> m_Trackings;
        HuntStatistics m_AllTargetsTotal;
        static DamageTrackManager* m_Instance;
        DamageTrackManager();
        DamageTrackManager operator=(DamageTrackManager const&);
        EntityDamageData* CalculateTotalDamage();

    public:
        static DamageTrackManager* GetInstance();

        void UpdateDamage(const EntityDamageData& damageData);
        HuntStatistics* GetHuntStatisticsBy(intptr_t target);
        void DeleteBy(intptr_t target);
        void ClearAllExcept(intptr_t* exceptions, size_t length);
    };
}
