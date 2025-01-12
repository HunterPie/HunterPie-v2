using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;

public class MHRCohoot : IEventDispatcher, IUpdatable<MHRCohootStructure>, IDisposable
{
    private int _kamuraCount;
    private int _elgadoCount;

    public int KamuraCount
    {
        get => _kamuraCount;
        private set
        {
            if (value != _kamuraCount)
            {
                _kamuraCount = value;
                this.Dispatch(_onKamuraCountChange, this);
            }
        }
    }

    public int ElgadoCount
    {
        get => _elgadoCount;
        private set
        {
            if (value != _elgadoCount)
            {
                _elgadoCount = value;
                this.Dispatch(_onElgadoCountChange, this);
            }
        }
    }

    public int MaxCount => 5;

    private readonly SmartEvent<MHRCohoot> _onKamuraCountChange = new();
    public event EventHandler<MHRCohoot> OnKamuraCountChange
    {
        add => _onKamuraCountChange.Hook(value);
        remove => _onKamuraCountChange.Unhook(value);
    }

    private readonly SmartEvent<MHRCohoot> _onElgadoCountChange = new();
    public event EventHandler<MHRCohoot> OnElgadoCountChange
    {
        add => _onElgadoCountChange.Hook(value);
        remove => _onElgadoCountChange.Unhook(value);
    }

    public void Update(MHRCohootStructure data)
    {
        ElgadoCount = data.ElgadoCount;
        KamuraCount = data.KamuraCount;
    }

    public void Dispose()
    {
        _onKamuraCountChange.Dispose();
        _onElgadoCountChange.Dispose();
    }
}