using HunterPie.Core.Game.Entity.Player.Vitals;

namespace HunterPie.Core.Game.Events;
public class HealthChangeEventArgs
{

    public double Health { get; }
    public double MaxHealth { get; }
    public double RecoverableHealth { get; }
    public double MaxPossibleHealth { get; }
    public double Heal { get; }

    public HealthChangeEventArgs(IHealthComponent component)
    {
        Health = component.Current;
        MaxHealth = component.Max;
        RecoverableHealth = component.RecoverableHealth;
        MaxPossibleHealth = component.MaxPossibleHealth;
        Heal = component.Heal;
    }
}