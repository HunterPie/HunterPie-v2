using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Core.Game.Services.Monster;
using HunterPie.Core.Game.Services.Monster.Events;
using TargetType = HunterPie.Core.Game.Enums.Target;

namespace HunterPie.Integrations.Datasources.Common.Monster;

internal class SimpleTargetDetectionService : ITargetDetectionService, IDisposable, IEventDispatcher
{
    private readonly IGame _game;
    private readonly object _lock = new();
    private IMonster? _cachedTarget;

    public IMonster? Target
    {
        get
        {
            lock (_lock)
                return _cachedTarget;
        }
        set
        {
            if (_cachedTarget == value)
                return;

            _cachedTarget = value;
            this.Dispatch(_onTargetChanged, new InferTargetChangedEventArgs(value));
        }
    }

    public SimpleTargetDetectionService(IGame game)
    {
        _game = game;
        HookEvents();
    }

    private readonly SmartEvent<InferTargetChangedEventArgs> _onTargetChanged = new();
    public event EventHandler<InferTargetChangedEventArgs> OnTargetChanged
    {
        add => _onTargetChanged.Hook(value);
        remove => _onTargetChanged.Unhook(value);
    }

    public TargetType Infer(IMonster monster)
    {
        if (_cachedTarget is not { } cached)
            return TargetType.None;

        return cached == monster
            ? TargetType.Self
            : TargetType.Another;
    }

    public void Dispose()
    {
        UnhookEvents();
    }

    private void HookEvents()
    {
        _game.OnMonsterSpawn += OnMonsterSpawn;
        _game.OnMonsterDespawn += OnMonsterDespawn;
    }

    private void UnhookEvents()
    {
        _game.OnMonsterSpawn -= OnMonsterSpawn;
        _game.OnMonsterDespawn -= OnMonsterDespawn;
    }

    private void OnMonsterSpawn(object? sender, IMonster e)
    {
        Target ??= e;

        e.OnHealthChange += OnMonsterHealthChange;
    }

    private void OnMonsterDespawn(object? sender, IMonster e)
    {
        Target = _game.Monsters.SingleOrNull();

        e.OnHealthChange -= OnMonsterHealthChange;
    }

    private void OnMonsterHealthChange(object? sender, EventArgs e)
    {
        if (sender is not IMonster monster)
            return;

        lock (_lock)
            Target = monster;
    }
}