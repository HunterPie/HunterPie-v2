#include "pch.h"
#include "Hooks.h"
#include "Core/Debug/logger.h"
#include "Core/Managers/Damage/manager.h"
#include "Core/Utils/addresses.h"
#include "Games/World/Utils/WorldUtils.h"
#include "libs/MinHook/MinHook.h"

namespace Games::World::Damage::Hooks
{
    DealDamageFunction ogDealDamage = nullptr;

    void DealDamage(
        Common::Monster* target,
        int damage,
        void* position,
        BOOL isTenderized,
        BOOL isCrit,
        int unk0,
        int unk1,
        char unk2,
        int attackId)
    {
        if (!ogDealDamage) {
            LOG("Unexpected call before hook initialization.");
            return;
        }
        ogDealDamage(target, damage, position, isTenderized, isCrit, unk0, unk1, unk2, attackId);

        LOG("Hit monster id: %d; damage: %d; atkId: %d", target->id, damage, attackId);

        // Only count in Large Monsters.
        if (!Utils::IsLargeMonster(target->id)) return;

        auto entity = Entity{
            0,     // We use 0 to indicate local player here.
            PLAYER,
        };

        auto entityData = EntityDamageData{
            reinterpret_cast<intptr_t>(target),
            entity,
            static_cast<float>(damage),
            // We cannot distinguish between raw / elemental here.
            0,
        };

        HunterPie::Core::Damage::DamageTrackManager::GetInstance()->UpdateDamage(entityData);
    }

    HRESULT DamageHooks::Init(uintptr_t* pointers)
    {
        if (ogDealDamage) {
            LOG("DealDamage function is already hooked. Original function at %p.", ogDealDamage);
            return ERROR_ALREADY_INITIALIZED;
        }

        ogDealDamage = nullptr;
        auto originalDealDamagePtr = reinterpret_cast<DealDamageFunction>(pointers[FUN_CALCULATE_ENTITY_DAMAGE]);

        LOG("Hook DealDamage function at %p", originalDealDamagePtr);
        auto status = MH_CreateHook(
            originalDealDamagePtr,
            static_cast<DealDamageFunction>(&DealDamage),       // cast for type-check
            reinterpret_cast<LPVOID*>(&ogDealDamage)
        );

        LOG("Hook DealDamage status: %s", MH_StatusToString(status));

        return status == MH_OK ? ERROR_SUCCESS : ERROR_HOOK_NOT_INSTALLED;
    }
}
