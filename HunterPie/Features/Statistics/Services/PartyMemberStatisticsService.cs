using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Features.Statistics.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Features.Statistics.Services;

#nullable enable
internal class PartyMemberStatisticsService : IHuntStatisticsService<PartyMemberModel>
{
    private readonly IPartyMember _partyMember;
    private readonly IContext _context;
    private readonly string _name;
    private readonly List<PlayerDamageFrameModel> _damages = new();
    private readonly Dictionary<string, Stack<TimeFrameModel>> _abnormalities = new();

    private float _currentDamage;
    private Weapon _weapon;


    public PartyMemberStatisticsService(IContext context, IPartyMember partyMember)
    {
        _partyMember = partyMember;
        _context = context;
        _name = partyMember.Name;
        _weapon = partyMember.Weapon;

        HookEvents();
    }

    public PartyMemberModel Export()
    {
        foreach (Stack<TimeFrameModel> timeFrames in _abnormalities.Values)
        {
            TimeFrameModel? lastTimeFrame = timeFrames.PopOrDefault();

            if (lastTimeFrame is null)
                continue;

            if (lastTimeFrame.IsRunning())
                lastTimeFrame = lastTimeFrame.End();

            timeFrames.Push(lastTimeFrame);
        }

        return new(
            Name: _name,
            Weapon: _weapon,
            Damages: _damages.ToArray(),
            Abnormalities: _abnormalities.Select(pair => new AbnormalityModel(Id: pair.Key, Activations: pair.Value.ToArray())).ToArray(),
            IsHunterPieUser: _partyMember.IsMyself
        );
    }

    private void HookEvents()
    {
        _partyMember.OnDamageDealt += OnDamageDealt;
        _partyMember.OnWeaponChange += OnWeaponChange;

        if (!_partyMember.IsMyself)
            return;

        _context.Game.Player.OnWeaponChange += OnWeaponChange;
        _context.Game.Player.OnAbnormalityStart += OnAbnormalityStart;
        _context.Game.Player.OnAbnormalityEnd += OnAbnormalityEnd;

        foreach (IAbnormality abnormality in _context.Game.Player.Abnormalities)
            HandleAbnormalityStart(abnormality);

    }

    private void UnhookEvents()
    {
        _partyMember.OnDamageDealt -= OnDamageDealt;
        _partyMember.OnWeaponChange -= OnWeaponChange;

        if (!_partyMember.IsMyself)
            return;

        _context.Game.Player.OnWeaponChange -= OnWeaponChange;
        _context.Game.Player.OnAbnormalityStart -= OnAbnormalityStart;
        _context.Game.Player.OnAbnormalityEnd -= OnAbnormalityEnd;
    }

    private void OnAbnormalityEnd(object? sender, IAbnormality e)
    {
        if (!_abnormalities.ContainsKey(e.Id))
            return;

        Stack<TimeFrameModel> timeFrames = _abnormalities[e.Id];

        TimeFrameModel? lastOccurrence = timeFrames.PopOrDefault();
        timeFrames.PushNotNull(lastOccurrence?.End());
    }

    private void OnAbnormalityStart(object? sender, IAbnormality e) => HandleAbnormalityStart(e);

    private void OnWeaponChange(object? sender, WeaponChangeEventArgs e) => _weapon = e.NewWeapon.Id;

    private void OnWeaponChange(object? sender, IPartyMember e) => _weapon = e.Weapon;

    private void OnDamageDealt(object? sender, IPartyMember e)
    {
        float hitDamage = e.Damage - _currentDamage;
        DateTime now = DateTime.UtcNow;

        _damages.Add(new PlayerDamageFrameModel(Damage: hitDamage, DealtAt: now));

        _currentDamage = e.Damage;
    }

    private void HandleAbnormalityStart(IAbnormality abnormality)
    {
        if (!_abnormalities.ContainsKey(abnormality.Id))
            _abnormalities[abnormality.Id] = new Stack<TimeFrameModel>();

        Stack<TimeFrameModel> timeFrames = _abnormalities[abnormality.Id];

        timeFrames.Push(TimeFrameModel.Start());
    }

    public void Dispose() => UnhookEvents();
}