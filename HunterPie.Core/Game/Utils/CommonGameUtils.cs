using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Utils;
public static class CommonGameUtils
{

    /// <summary>
    /// Returns whether weapon is considered melee or not
    /// </summary>
    /// <param name="weapon">Weapon id</param>
    /// <returns>True if it's melee</returns>
    public static bool IsMelee(this Weapon weapon) => !(weapon is Weapon.Bow or Weapon.HeavyBowgun or Weapon.LightBowgun or Weapon.None);

}