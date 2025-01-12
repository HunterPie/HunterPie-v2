using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Game.Chat;

namespace HunterPie.Integrations.Datasources.Common.Entity.Chat;
public abstract class CommonChat : IChat, IEventDispatcher, IDisposable
{
    public abstract List<IChatMessage> Messages { get; }
    public abstract bool IsChatOpen { get; protected set; }

    protected readonly SmartEvent<IChatMessage> _onNewChatMessage = new();
    public event EventHandler<IChatMessage> OnNewChatMessage
    {
        add => _onNewChatMessage.Hook(value);
        remove => _onNewChatMessage.Unhook(value);
    }

    protected readonly SmartEvent<IChat> _onChatOpen = new();
    public event EventHandler<IChat> OnChatOpen
    {
        add => _onChatOpen.Hook(value);
        remove => _onChatOpen.Unhook(value);
    }

    public void Dispose()
    {
        IDisposable[] events = { _onNewChatMessage, _onChatOpen };

        events.DisposeAll();
    }
}