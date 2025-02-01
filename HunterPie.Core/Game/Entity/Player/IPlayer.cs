using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Events;
using System;
using System.Collections.Generic;

namespace HunterPie.Core.Game.Entity.Player;

public interface IPlayer
{
    public string Name { get; }
    public int HighRank { get; }
    public int MasterRank { get; }
    public int StageId { get; }
    public bool InHuntingZone { get; }
    public IParty Party { get; }
    public IReadOnlyCollection<IAbnormality> Abnormalities { get; }
    public IHealthComponent Health { get; }
    public IStaminaComponent Stamina { get; }
    public IWeapon Weapon { get; }


    public event EventHandler<EventArgs> OnLogin;
    public event EventHandler<EventArgs> OnLogout;
    public event EventHandler<EventArgs> OnDeath;
    public event EventHandler<EventArgs> OnActionUpdate;
    public event EventHandler<EventArgs> OnStageUpdate;
    public event EventHandler<EventArgs> OnVillageEnter;
    public event EventHandler<EventArgs> OnVillageLeave;
    public event EventHandler<EventArgs> OnAilmentUpdate;
    public event EventHandler<WeaponChangeEventArgs> OnWeaponChange;
    public event EventHandler<IAbnormality> OnAbnormalityStart;
    public event EventHandler<IAbnormality> OnAbnormalityEnd;
    public event EventHandler<LevelChangeEventArgs> OnLevelChange;
}