using HunterPie.Core.Native.IPC.Models;
using System;
using System.Linq;
using MessageHandlerProvider = System.Collections.Generic.Dictionary<HunterPie.Core.Native.IPC.Models.IPCMessageType, System.Collections.Generic.List<HunterPie.Core.Native.IPC.Handlers.IMessageHandler>>;

namespace HunterPie.Core.Native.IPC.Handlers;

public static class MessageHandlerManager
{

    private static readonly Lazy<MessageHandlerProvider> _handlerProvider = new(InitProvider);
    private static MessageHandlerProvider HandlerProvider => _handlerProvider.Value;

    public static void Dispatch(IPCMessageType type, byte[] message)
    {
        if (!HandlerProvider.ContainsKey(type))
            return;

        foreach (IMessageHandler handler in HandlerProvider[type])
            handler.Handle(message);
    }

    private static MessageHandlerProvider InitProvider()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetInterface(nameof(IMessageHandler)) is not null)
            .Select(@class => (IMessageHandler)Activator.CreateInstance(@class))
            .GroupBy(handler => handler.Type)
            .ToDictionary(group => group.Key, group => group.ToList());
    }
}