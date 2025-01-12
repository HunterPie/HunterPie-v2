using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
public class MHWArgosy : IEventDispatcher, IDisposable, IUpdatable<MHWArgosyData>
{

    private int _daysLeft;

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

    public bool IsInTown { get; private set; }

    private readonly SmartEvent<MHWArgosy> _onDaysLeftChange = new();
    public event EventHandler<MHWArgosy> OnDaysLeftChange
    {
        add => _onDaysLeftChange.Hook(value);
        remove => _onDaysLeftChange.Unhook(value);
    }

    public void Dispose()
    {
        _onDaysLeftChange.Dispose();
    }

    public void Update(MHWArgosyData data)
    {
        IsInTown = data.IsInTown;
        DaysLeft = data.DaysLeft;
    }
}