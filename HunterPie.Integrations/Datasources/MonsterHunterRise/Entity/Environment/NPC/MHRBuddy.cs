using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.NPC;

public class MHRBuddy : IEventDispatcher, IUpdatable<MHRBuddyData>, IDisposable
{
    private string _name = string.Empty;
    private int _level;

    public string Name
    {
        get => _name;
        private set
        {
            if (value != _name)
            {
                _name = value;
                this.Dispatch(_onNameChange, this);
            }
        }
    }

    public int Level
    {
        get => _level;
        private set
        {
            if (value != _level)
            {
                _level = value;
                this.Dispatch(_onLevelChange, this);
            }
        }
    }

    private readonly SmartEvent<MHRBuddy> _onNameChange = new();
    public event EventHandler<MHRBuddy> OnNameChange
    {
        add => _onNameChange.Hook(value);
        remove => _onNameChange.Unhook(value);
    }

    private readonly SmartEvent<MHRBuddy> _onLevelChange = new();
    public event EventHandler<MHRBuddy> OnLevelChange
    {
        add => _onLevelChange.Hook(value);
        remove => _onLevelChange.Unhook(value);
    }

    void IUpdatable<MHRBuddyData>.Update(MHRBuddyData data)
    {
        Name = data.Name;
        Level = data.Level;
    }

    public void Dispose()
    {
        _onNameChange.Dispose();
        _onLevelChange.Dispose();
    }
}