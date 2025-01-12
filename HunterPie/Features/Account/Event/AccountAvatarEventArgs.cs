using System;

namespace HunterPie.Features.Account.Event;
public class AccountAvatarEventArgs : EventArgs
{
    public string AvatarUrl { get; init; }
}