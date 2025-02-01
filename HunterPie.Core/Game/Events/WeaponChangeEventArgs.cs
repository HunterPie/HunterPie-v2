using HunterPie.Core.Game.Entity.Player.Classes;
using System;

namespace HunterPie.Core.Game.Events;
public class WeaponChangeEventArgs : EventArgs
{
    public IWeapon OldWeapon { get; }
    public IWeapon NewWeapon { get; }

    public WeaponChangeEventArgs(IWeapon oldWeapon, IWeapon newWeapon)
    {
        OldWeapon = oldWeapon;
        NewWeapon = newWeapon;
    }
}