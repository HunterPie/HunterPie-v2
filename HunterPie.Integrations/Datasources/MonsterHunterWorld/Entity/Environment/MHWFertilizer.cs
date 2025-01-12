using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment;

public class MHWFertilizer : IEventDispatcher, IUpdatable<MHWFertilizerStructure>, IDisposable
{
    private Fertilizer _id;
    public Fertilizer Id
    {
        get => _id;
        private set
        {
            if (value != _id)
            {
                _id = value;
                this.Dispatch(_onIdChange, this);
            }
        }
    }

    private int _count;
    public int Count
    {
        get => _count;
        private set
        {
            if (value != _count)
            {
                _count = value;
                this.Dispatch(_onCountChange, this);
            }
        }
    }

    private readonly SmartEvent<MHWFertilizer> _onIdChange = new();
    public event EventHandler<MHWFertilizer> OnIdChange
    {
        add => _onIdChange.Hook(value);
        remove => _onIdChange.Unhook(value);
    }

    private readonly SmartEvent<MHWFertilizer> _onCountChange = new();
    public event EventHandler<MHWFertilizer> OnCountChange
    {
        add => _onCountChange.Hook(value);
        remove => _onCountChange.Unhook(value);
    }

    public void Update(MHWFertilizerStructure data)
    {
        Id = (Fertilizer)data.Id;
        Count = data.Amount;
    }

    public void Dispose()
    {
        _onIdChange.Dispose();
        _onCountChange.Dispose();
    }
}