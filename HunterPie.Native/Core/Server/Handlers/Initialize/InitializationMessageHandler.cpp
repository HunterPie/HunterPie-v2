#include "pch.h"
#include "InitializationMessageHandler.h"
#include "Games/World/Damage/hooks.h"
#include "Games/Rise/Damage/hooks.h"
#include "libs/MinHook/MinHook.h"

using namespace Core::Server;
using namespace Core::Server::Models;
using namespace Core::Server::Handlers;

void OnRequestIPCInitialization(RequestIPCInitializationMessage* message)
{
    ResponseIPCInitializationMessage response{};
    WITH(response)
    {
        it.type = INIT_IPC_MEMORY_ADDRESSES;
        it.version = 2;
        it.hresult = ERROR_SUCCESS;
    }

    MH_Initialize();
    switch (message->hostType) {
    case IPCInitializationHostType::MHWorld:
        response.hresult = Games::World::Damage::Hooks::DamageHooks().Init(message->addresses);
        break;
    case IPCInitializationHostType::MHRise:
        response.hresult = Games::Rise::Damage::Hook::DamageHooks().Init(message->addresses);
        break;
    default:
        response.hresult = E_INVALIDARG;
        break;
    }

    IPCService::GetInstance()->SendIPCMessage(&response, sizeof(ResponseIPCInitializationMessage));
}

void OnRequestInitMHHooks(IPCMessage* message)
{
    MH_STATUS status = MH_EnableHook(MH_ALL_HOOKS);

    ResponseInitMHHooksMessage response{};

    WITH(response)
    {
        it.type = INIT_MH_HOOKS;
        it.version = 1;
        it.status = (int)status;
    }

    IPCService::GetInstance()->SendIPCMessage(&response, sizeof(ResponseInitMHHooksMessage));
}

void InitializationMessageHandler::Initialize()
{
    WITH_INSTANCE(IPCService::GetInstance())
    {
        it->RegisterMessageHandler(INIT_IPC_MEMORY_ADDRESSES, &OnRequestIPCInitialization);
        it->RegisterMessageHandler(INIT_MH_HOOKS, &OnRequestInitMHHooks);
    }
}

const char* InitializationMessageHandler::GetName()
{
    return NAMEOF(InitializationMessageHandler);
}