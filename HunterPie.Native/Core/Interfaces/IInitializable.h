#pragma once

namespace Core
{
    class IInitializable
    {
    public:
        virtual HRESULT Init(uintptr_t* pointers) = 0;
        virtual ~IInitializable() = default;
    };
}
