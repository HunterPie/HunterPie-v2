using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;
public class SharpnessEventArgs : EventArgs
{
    /// <summary>
    /// Current sharpness level
    /// </summary>
    public Sharpness Sharpness { get; }

    /// <summary>
    /// Current sharpness value
    /// </summary>
    public int CurrentSharpness { get; }

    /// <summary>
    /// Current weapon sharpness thresholds
    /// </summary>
    public int[] SharpnessThresholds { get; }

    /// <summary>
    /// Thresholds for the current level
    /// </summary>
    public int Threshold { get; }

    /// <summary>
    /// Maximum sharpness for the current level
    /// </summary>
    public int MaxSharpness { get; }

    public SharpnessEventArgs(IMeleeWeapon weapon)
    {
        Sharpness = weapon.Sharpness;
        CurrentSharpness = weapon.CurrentSharpness;
        SharpnessThresholds = weapon.SharpnessThresholds;
        Threshold = weapon.Threshold;
        MaxSharpness = weapon.MaxSharpness;
    }
}