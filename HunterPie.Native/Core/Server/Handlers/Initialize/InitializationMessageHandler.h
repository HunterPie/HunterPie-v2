#pragma once
#include "../MessageHandler.h"
#include "../../IPCService.h"
#include "../../Models/Messages.h"

namespace Core::Server
{
    namespace Models
    {
        enum class IPCInitializationHostType : uint32_t
        {
            Invalid = 0,
            MHWorld,
            MHRise,
        };

        struct RequestIPCInitializationMessage : IPCMessage
        {
            IPCInitializationHostType hostType;
            uintptr_t addresses[256];
        };

        struct ResponseIPCInitializationMessage : IPCMessage
        {
            HRESULT hresult;
        };

        struct ResponseInitMHHooksMessage : IPCMessage
        {
            int status;
        };
    }

    namespace Handlers
    {
        class InitializationMessageHandler : public MessageHandler
        {
        public:
            virtual void Initialize();
            virtual const char* GetName();
        };
    }
}
