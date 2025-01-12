using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;

public class MHRSubmarine : IEventDispatcher, IUpdatable<MHRSubmarineData>, IDisposable
{
    private int _count;
    private int _daysLeft;
    private bool _isUnlocked;

    public int Count
    {
        get => _count;
        private set
        {
            if (value != _count)
            {
                _count = value;
                this.Dispatch(_onItemCountChange, this);
            }
        }
    }

    public int MaxCount { get; private set; } = 20;

    public int DaysLeft
    {
        get => _daysLeft;
        private set
        {
            if (value != _daysLeft)
            {
                _daysLeft = value;
                this.Dispatch(_onDaysLeftChange, this);
            }
        }
    }

    public bool IsUnlocked
    {
        get => _isUnlocked;
        private set
        {
            if (value != _isUnlocked)
            {
                _isUnlocked = value;
                this.Dispatch(_onLockStateChange, this);
            }
        }
    }

    private readonly SmartEvent<MHRSubmarine> _onItemCountChange = new();
    public event EventHandler<MHRSubmarine> OnItemCountChange
    {
        add => _onItemCountChange.Hook(value);
        remove => _onItemCountChange.Unhook(value);
    }

    private readonly SmartEvent<MHRSubmarine> _onDaysLeftChange = new();
    public event EventHandler<MHRSubmarine> OnDaysLeftChange
    {
        add => _onDaysLeftChange.Hook(value);
        remove => _onDaysLeftChange.Unhook(value);
    }

    private readonly SmartEvent<MHRSubmarine> _onLockStateChange = new();
    public event EventHandler<MHRSubmarine> OnLockStateChange
    {
        add => _onLockStateChange.Hook(value);
        remove => _onLockStateChange.Unhook(value);
    }

    public void Update(MHRSubmarineData data)
    {
        DaysLeft = data.Data.DaysLeft;
        MaxCount = data.Items.Length;
        Count = data.Items.Count(item => item.IsNotEmpty());
        IsUnlocked = data.Data.Buddy != 0;
    }

    public void Dispose()
    {
        _onItemCountChange.Dispose();
        _onDaysLeftChange.Dispose();
        _onLockStateChange.Dispose();
    }
}