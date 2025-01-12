using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
public class MHWHarvestBox : IEventDispatcher, IUpdatable<MHWHarvestBoxData>, IDisposable
{
    private int _count;

    public int Count
    {
        get => _count;
        private set
        {
            if (value != _count)
            {
                _count = value;
                this.Dispatch(_onItemsCountChange, this);
            }
        }
    }

    public readonly MHWFertilizer[] Fertilizers = { new(), new(), new(), new() };

    private readonly SmartEvent<MHWHarvestBox> _onItemsCountChange = new();
    public event EventHandler<MHWHarvestBox> OnItemsCountChange
    {
        add => _onItemsCountChange.Hook(value);
        remove => _onItemsCountChange.Unhook(value);
    }

    public void Update(MHWHarvestBoxData data)
    {
        Count = data.Items.Sum(item => item.Amount > 0 ? 1 : 0);

        for (int i = 0; i < Fertilizers.Length; i++)
            Fertilizers[i].Update(data.Fertilizers[i]);
    }

    public void Dispose()
    {
        foreach (MHWFertilizer fertilizer in Fertilizers)
            fertilizer.Dispose();

        _onItemsCountChange.Dispose();
    }
}