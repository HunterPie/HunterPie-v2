using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Features.Statistics.Models;
using System;
using System.Collections.Generic;

namespace HunterPie.Features.Statistics.Services;

#nullable enable
internal class MonsterStatisticsService : IHuntStatisticsService<MonsterModel>
{
    private readonly IContext _context;
    private readonly IMonster _monster;

    private int _monsterId;
    private float? _startHealth;
    private readonly Stack<TimeFrameModel> _enrages = new();

    private DateTime? _huntStart;
    private DateTime? _huntEnd;
    private float _maxHealth;
    private Crown _crown;
    private MonsterHuntType _huntType;

    public MonsterStatisticsService(IContext context, IMonster monster)
    {
        _context = context;
        _monster = monster;
        _monsterId = _monster.Id;
        _maxHealth = monster.MaxHealth;

        HookEvents();
    }

    public MonsterModel Export()
    {
        TimeFrameModel? lastEnrage = _enrages.PopOrDefault();

        if (_huntStart is { })
            _enrages.PushNotNull(
                lastEnrage?.IsRunning() == true
                    ? lastEnrage.End()
                    : lastEnrage
                );

        _huntEnd = _huntStart is { } ? DateTime.UtcNow : null;

        return new MonsterModel(
            Id: _monsterId,
            MaxHealth: _maxHealth,
            Crown: _crown,
            Enrage: new MonsterStatusModel(
                Activations: _enrages.ToArray()
            ),
            HuntStartedAt: _huntStart,
            HuntFinishedAt: _huntEnd,
            HuntType: _huntType
        );
    }

    private void HookEvents()
    {
        _monster.OnSpawn += OnMonsterSpawn;
        _monster.OnEnrageStateChange += OnEnrageStateChange;
        _monster.OnCapture += OnCapture;
        _monster.OnDeath += OnDeath;
        _monster.OnCrownChange += OnCrownChange;
        _monster.OnHealthChange += OnHealthChange;
    }

    private void UnhookEvents()
    {
        _monster.OnSpawn -= OnMonsterSpawn;
        _monster.OnEnrageStateChange -= OnEnrageStateChange;
        _monster.OnCapture -= OnCapture;
        _monster.OnDeath -= OnDeath;
        _monster.OnCrownChange -= OnCrownChange;
        _monster.OnHealthChange -= OnHealthChange;
    }

    private void OnMonsterSpawn(object? sender, EventArgs e)
    {
        _monsterId = _monster.Id;
    }

    private void OnHealthChange(object? sender, EventArgs e)
    {
        _maxHealth = Math.Max(_maxHealth, _monster.MaxHealth);

        if (Math.Abs(_monster.Health - _monster.MaxHealth) < 0.1)
            return;

        if (_huntStart is not null || _startHealth is not null)
            return;

        _huntStart = DateTime.UtcNow;
        _startHealth = _monster.Health;
    }

    private void OnCrownChange(object? sender, EventArgs e)
    {
        _crown = _monster.Crown;
    }

    private void OnDeath(object? sender, EventArgs e)
    {
        _huntEnd = DateTime.UtcNow;
        _huntType = MonsterHuntType.Slay;
    }

    private void OnCapture(object? sender, EventArgs e)
    {
        _huntEnd = DateTime.UtcNow;
        _huntType = MonsterHuntType.Capture;
    }

    private void OnEnrageStateChange(object? sender, EventArgs e)
    {
        if (_huntStart is not { })
            return;

        TimeFrameModel? lastEnrage = _enrages.PopOrDefault();

        _enrages.PushNotNull(lastEnrage?.End());

        if (!_monster.IsEnraged)
            return;

        _enrages.Push(TimeFrameModel.Start());
    }

    public void Dispose() => UnhookEvents();
}