using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Party;

namespace HunterPie.Integrations.Datasources.Common.Entity.Party;
public abstract class CommonParty : IParty, IEventDispatcher, IDisposable
{
    public abstract int Size { get; protected set; }
    public abstract int MaxSize { get; protected set; }
    public abstract List<IPartyMember> Members { get; }

    protected readonly SmartEvent<IPartyMember> _onMemberJoin = new();
    public event EventHandler<IPartyMember> OnMemberJoin
    {
        add => _onMemberJoin.Hook(value);
        remove => _onMemberJoin.Unhook(value);
    }

    protected readonly SmartEvent<IPartyMember> _onMemberLeave = new();
    public event EventHandler<IPartyMember> OnMemberLeave
    {
        add => _onMemberLeave.Hook(value);
        remove => _onMemberLeave.Unhook(value);
    }

    public virtual void Dispose()
    {
        IDisposable[] events = { _onMemberJoin, _onMemberLeave, };

        events.DisposeAll();

        Members.TryCast<IDisposable>()
               .DisposeAll();
    }
}