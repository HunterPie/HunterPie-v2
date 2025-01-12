#pragma once
#include "damage.h"
#include "../Common/Monster.h"
#include "Core/Interfaces/IInitializable.h"

using namespace Core;
using namespace Games::Rise::Common;

namespace Games::Rise::Damage::Hook
{
    MHREntityData* CalculateEntityDamage(
        intptr_t arg1,
        Monster* target,
        intptr_t arg3,
        intptr_t arg4,
        intptr_t arg5,
        void* arg6
    );

    class DamageHooks : public IInitializable
    {
    public:
        virtual HRESULT Init(uintptr_t* pointers);
    };
}
