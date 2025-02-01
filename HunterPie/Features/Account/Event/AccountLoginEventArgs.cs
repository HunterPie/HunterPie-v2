using HunterPie.Features.Account.Model;
using System;

namespace HunterPie.Features.Account.Event;
internal class AccountLoginEventArgs : EventArgs
{
    public required UserAccount Account { get; init; }
}