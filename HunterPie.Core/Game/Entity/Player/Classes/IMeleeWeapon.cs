using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player.Classes;
public interface IMeleeWeapon
{
    public Sharpness Sharpness { get; }
    public int CurrentSharpness { get; }
    public int MaxSharpness { get; }
    public int Threshold { get; }

    public int[]? SharpnessThresholds { get; }

    public event EventHandler<SharpnessEventArgs> OnSharpnessChange;
    public event EventHandler<SharpnessEventArgs> OnSharpnessLevelChange;
}