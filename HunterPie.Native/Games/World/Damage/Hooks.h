#pragma once
#include "Core/Interfaces/IInitializable.h"
#include "../Common/Monster.h"

namespace Games::World::Damage::Hooks
{
    typedef void (*DealDamageFunction)(Common::Monster* target, int damage, void* position, BOOL isTenderized, BOOL isCrit, int unk0, int unk1, char unk2, int attackId);

    void DealDamage(Common::Monster* target, int damage, void* position, BOOL isTenderized, BOOL isCrit, int unk0, int unk1, char unk2, int attackId);

    class DamageHooks : public Core::IInitializable
    {
    public:
        HRESULT Init(uintptr_t* pointers) override;
    };
};
