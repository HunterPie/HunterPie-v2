using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.NPC;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;

public class MHRTrainingDojo : IEventDispatcher, IUpdatable<MHRTrainingDojoData>, IDisposable
{
    private int _rounds;
    private int _boosts;
    private int _buddiesCount;

    public int Rounds
    {
        get => _rounds;
        private set
        {
            if (value != _rounds)
            {
                _rounds = value;
                this.Dispatch(_onRoundsLeftChange, this);
            }
        }
    }
    public int MaxRounds { get; private set; }

    public int Boosts
    {
        get => _boosts;
        private set
        {
            if (value != _boosts)
            {
                _boosts = value;
                this.Dispatch(_onBoostsLeftChange, this);
            }
        }
    }
    public int MaxBoosts { get; private set; }

    public int BuddiesCount
    {
        get => _buddiesCount;
        private set
        {
            if (value != _buddiesCount)
            {
                _buddiesCount = value;
                this.Dispatch(_onBuddyCountChange, this);
            }
        }
    }

    public readonly MHRBuddy[] Buddies = { new(), new(), new(), new(), new(), new() };

    private readonly SmartEvent<MHRTrainingDojo> _onRoundsLeftChange = new();
    public event EventHandler<MHRTrainingDojo> OnRoundsLeftChange
    {
        add => _onRoundsLeftChange.Hook(value);
        remove => _onRoundsLeftChange.Unhook(value);
    }

    private readonly SmartEvent<MHRTrainingDojo> _onBoostsLeftChange = new();
    public event EventHandler<MHRTrainingDojo> OnBoostsLeftChange
    {
        add => _onBoostsLeftChange.Hook(value);
        remove => _onBoostsLeftChange.Unhook(value);
    }

    private readonly SmartEvent<MHRTrainingDojo> _onBuddyCountChange = new();
    public event EventHandler<MHRTrainingDojo> OnBuddyCountChange
    {
        add => _onBuddyCountChange.Hook(value);
        remove => _onBuddyCountChange.Unhook(value);
    }

    public void Update(MHRTrainingDojoData data)
    {
        BuddiesCount = data.BuddiesCount;

        for (int i = 0; i < Buddies.Length; i++)
        {
            IUpdatable<MHRBuddyData> buddy = Buddies[i];
            buddy.Update(data.Buddies[i]);
        }

        MaxRounds = data.MaxRounds;
        Rounds = data.Rounds;
        MaxBoosts = data.MaxBoosts;
        Boosts = data.Boosts;

    }

    public void Dispose()
    {
        Buddies.DisposeAll();
        _onRoundsLeftChange.Dispose();
        _onBoostsLeftChange.Dispose();
        _onBuddyCountChange.Dispose();
    }
}