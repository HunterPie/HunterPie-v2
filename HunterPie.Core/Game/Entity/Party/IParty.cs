using System;
using System.Collections.Generic;

namespace HunterPie.Core.Game.Entity.Party;

public interface IParty
{
    public int Size { get; }
    public int MaxSize { get; }
    public List<IPartyMember> Members { get; }

    public event EventHandler<IPartyMember> OnMemberJoin;
    public event EventHandler<IPartyMember> OnMemberLeave;
}