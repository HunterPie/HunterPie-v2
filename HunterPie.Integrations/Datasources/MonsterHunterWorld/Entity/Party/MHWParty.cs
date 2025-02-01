using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.Common.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Party;

public sealed class MHWParty : CommonParty
{
    public const int MAXIMUM_SIZE = 4;

    private readonly Dictionary<long, MHWPartyMember> _partyMembers = new(MAXIMUM_SIZE);

    public override int Size
    {
        get
        {
            lock (_partyMembers)
                return _partyMembers.Count;
        }
        protected set => throw new NotSupportedException();
    }

    public override int MaxSize
    {
        get => MAXIMUM_SIZE;
        protected set => throw new NotSupportedException();
    }

    public override List<IPartyMember> Members
    {
        get
        {
            lock (_partyMembers)
                return _partyMembers.Values.ToList<IPartyMember>();
        }
    }

    public void Update(long memberAddress, MHWPartyMemberData data)
    {
        lock (_partyMembers)
        {
            if (!_partyMembers.TryGetValue(memberAddress, out MHWPartyMember? member))
            {
                Add(memberAddress, data);
                return;
            }

            member.Update(data);
        }
    }

    public void Update(long memberAddress, EntityDamageData data)
    {
        lock (_partyMembers)
        {
            if (!_partyMembers.TryGetValue(memberAddress, out MHWPartyMember? member))
                return;

            member.Update(data);
        }
    }

    private void Add(long memberAddress, MHWPartyMemberData data)
    {
        var member = new MHWPartyMember();
        member.Update(data);

        _partyMembers.Add(memberAddress, member);

        this.Dispatch(_onMemberJoin, member);
    }

    public void Remove(long memberAddress)
    {
        MHWPartyMember? member;
        lock (_partyMembers)
            if (!_partyMembers.Remove(memberAddress, out member))
                return;

        this.Dispatch(_onMemberLeave, member);

        member.Dispose();
    }

    public void ClearExcept(long memberAddress)
    {
        lock (_partyMembers)
        {
            long[] addresses = _partyMembers.Keys.Where(it => it != memberAddress)
                .ToArray();

            foreach (long address in addresses)
                Remove(address);
        }
    }
}