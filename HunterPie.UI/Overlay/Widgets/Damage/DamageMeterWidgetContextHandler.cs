using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Architecture.Brushes;
using HunterPie.UI.Overlay.Widgets.Damage.Helpers;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using HunterPie.UI.Settings.Converter;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using ObservableColor = HunterPie.Core.Settings.Types.Color;
using Range = HunterPie.Core.Settings.Types.Range;

namespace HunterPie.UI.Overlay.Widgets.Damage;

public class DamageMeterWidgetContextHandler : IContextHandler
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly MeterViewModel _viewModel;
    private readonly MeterView _view;
    private readonly IContext _context;
    private readonly Dictionary<IPartyMember, MemberInfo> _members = new();
    private readonly Dictionary<IPartyMember, DamageBarViewModel> _pets = new();
    private double _lastTimeElapsed;

    public DamageMeterWidgetContextHandler(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(context.Process.Type);

        _view = new MeterView(config.DamageMeterWidget);
        _ = WidgetManager.Register<MeterView, DamageMeterWidgetConfig>(_view);

        _viewModel = _view.ViewModel;
        _context = context;

        HookEvents();
        UpdateData();
    }

    private void UpdateData()
    {
        _viewModel.Pets.Name = Localization.QueryString("//Strings/Client/Overlay/String[@Id='DAMAGE_METER_OTOMOS_NAME_STRING']");
        _viewModel.InHuntingZone = _context.Game.Player.InHuntingZone;
        _viewModel.MaxDeaths = _context.Game.Quest?.MaxDeaths ?? 0;
        _viewModel.Deaths = _context.Game.Quest?.Deaths ?? 0;
        _viewModel.TimeElapsed = _context.Game.TimeElapsed;

        foreach (IPartyMember member in _context.Game.Player.Party.Members)
            HandleAddMember(member);

        _viewModel.HasPetsToBeDisplayed = _pets.Keys.Count > 0;
    }

    public void HookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin += OnMemberJoin;
        _context.Game.Player.Party.OnMemberLeave += OnMemberLeave;
        _context.Game.OnTimeElapsedChange += OnTimeElapsedChange;
        _context.Game.Player.OnStageUpdate += OnStageUpdate;
        _context.Game.OnQuestStart += OnQuestStart;
        _context.Game.OnQuestEnd += OnQuestEnd;
    }

    public void UnhookEvents()
    {
        _context.Game.Player.Party.OnMemberJoin -= OnMemberJoin;
        _context.Game.Player.Party.OnMemberLeave -= OnMemberLeave;
        _context.Game.OnTimeElapsedChange -= OnTimeElapsedChange;
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;
        _context.Game.OnQuestStart -= OnQuestStart;
        _context.Game.OnQuestEnd -= OnQuestEnd;

        foreach (IPartyMember member in _members.Keys.ToArray())
            HandleRemoveMember(member);

        if (_context.Game.Quest is { } quest)
            quest.OnDeathCounterChange -= OnDeathCounterChange;

        _ = WidgetManager.Unregister<MeterView, DamageMeterWidgetConfig>(_view);
    }

    #region Events
    private void OnQuestStart(object? sender, IQuest e)
    {
        e.OnDeathCounterChange += OnDeathCounterChange;
        _viewModel.MaxDeaths = e.MaxDeaths;
        _viewModel.Deaths = e.Deaths;
    }

    private void OnQuestEnd(object? sender, QuestEndEventArgs e)
    {
        e.Quest.OnDeathCounterChange -= OnDeathCounterChange;
    }

    private void OnDeathCounterChange(object? sender, CounterChangeEventArgs e)
    {
        _viewModel.MaxDeaths = e.Max;
        _viewModel.Deaths = e.Current;
    }

    private void OnStageUpdate(object? sender, EventArgs e)
    {
        bool inHuntingZone = _context.Game.Player.InHuntingZone;
        _viewModel.InHuntingZone = inHuntingZone;

        _view.Dispatcher.BeginInvoke(() =>
        {
            foreach (IPartyMember member in _members.Keys)
                RemovePlayer(member);

            _members.Clear();

            if (_context.Game.Player.InHuntingZone)
                foreach (IPartyMember member in _context.Game.Player.Party.Members)
                    HandleAddMember(member);
        });
    }

    private void GetPlayerPoints(bool isTimerReset)
    {
        foreach ((IPartyMember member, MemberInfo memberInfo) in _members)
        {
            var points = (ChartValues<ObservablePoint>)memberInfo.Series.Values;
            PlayerViewModel vm = memberInfo.ViewModel;

            float totalDamage = _members.Keys.Sum(m => m.Damage);
            vm.Bar.Percentage = totalDamage > 0 ? member.Damage / totalDamage * 100 : 0;

            if (isTimerReset)
                // If there is a timer reset, IGame.TimeElapsed may experience sudden change.
                // This may occur when we are switching from local timer to game timer.
                points.Clear();

            double newDps = CalculateDpsByConfiguredStrategy(memberInfo);
            vm.IsIncreasing = newDps > vm.DPS;
            vm.DPS = newDps;
            ObservablePoint point = CalculatePointByConfiguredStrategy(vm);
            points.Add(point);
        }
    }

    private void CalculatePetsDamage()
    {
        _viewModel.Pets.TotalDamage = _pets.Keys.Sum(pet => pet.Damage);

        foreach ((IPartyMember pet, DamageBarViewModel vm) in _pets)
            vm.Percentage = pet.Damage / (double)_viewModel.Pets.TotalDamage * 100;
    }

    private void OnMemberJoin(object? sender, IPartyMember e) =>
        _view.Dispatcher.BeginInvoke(() => HandleAddMember(e));

    private void OnMemberLeave(object? sender, IPartyMember e) =>
        _view.Dispatcher.BeginInvoke(() => HandleRemoveMember(e));

    private void OnTimeElapsedChange(object? sender, TimeElapsedChangeEventArgs e)
    {
        _viewModel.TimeElapsed = e.TimeElapsed;
        bool isTimerReset = e.IsTimerReset;

        if (!isTimerReset && (int)(e.TimeElapsed - _lastTimeElapsed) == 0)
            return;

        if (isTimerReset)
            // If the timer has just been reset, it usually means the local timer is being replaced with real game timer.
            // Note the info of party members in the current hunting party can be loaded before the real game timer gets ready in MHW.
            // Use 0 sec as other player's join time in this case to prevent a very large dps result.
            // We don't know when the others have joined after all.
            foreach ((IPartyMember member, MemberInfo memberInfo) in _members)
                memberInfo.JoinedAt = member.IsMyself ? e.TimeElapsed : 0;

        _view.Dispatcher.BeginInvoke(() =>
        {
            GetPlayerPoints(isTimerReset);
            CalculatePetsDamage();
            _viewModel.SortMembers();
        });

        _lastTimeElapsed = _viewModel.TimeElapsed;
    }

    private void OnDamageDealt(object? sender, IPartyMember e)
    {
        MemberInfo member = _members[e];
        PlayerViewModel vm = member.ViewModel;

        if (member.FirstHitAt == -1)
            member.FirstHitAt = _viewModel.TimeElapsed;

        double newDps = CalculateDpsByConfiguredStrategy(member);
        vm.IsIncreasing = newDps > vm.DPS;
        vm.Damage = e.Damage;
        vm.DPS = newDps;
    }

    private void OnWeaponChange(object? sender, IPartyMember e)
    {
        PlayerViewModel member = _members[e].ViewModel;
        member.Weapon = e.Weapon;
    }
    #endregion

    #region Helpers
    private Series BuildPlayerSeries(string name, string color)
    {
        ChartValues<ObservablePoint> points = new();

        var actualColor = (Color)ColorConverter.ConvertFromString(color);
        var series = new LineSeries
        {
            Title = name,
            Stroke = new SolidColorBrush(actualColor),
            Fill = ColorFadeGradient.FromColor(actualColor),
            PointGeometry = null,
            Values = points
        };
        Binding smoothingBinding = VisualConverterHelper.CreateBinding(_viewModel.Settings.PlotLineSmoothing, nameof(Range.Current));
        Binding thicknessBinding =
            VisualConverterHelper.CreateBinding(_viewModel.Settings.PlotLineThickness, nameof(Range.Current));

        series.SetBinding(Series.StrokeThicknessProperty, thicknessBinding);
        series.SetBinding(LineSeries.LineSmoothnessProperty, smoothingBinding);

        _viewModel.Series.Add(series);
        return series;
    }

#nullable enable
    private void HandleAddMember(IPartyMember member)
    {
        Action<IPartyMember>? func = member.Type switch
        {
            MemberType.Player => AddPlayer,
            MemberType.Pet => AddPet,
            _ => null,
        };

        func?.Invoke(member);
    }

    private void HandleRemoveMember(IPartyMember member)
    {
        Action<IPartyMember>? func = member.Type switch
        {
            MemberType.Player => RemovePlayer,
            MemberType.Pet => RemovePet,
            _ => null,
        };

        func?.Invoke(member);
    }
#nullable restore

    private void AddPet(IPartyMember pet)
    {
        if (_pets.ContainsKey(pet))
            return;

        ObservableColor playerColor = PlayerConfigHelper.GetColorFromPlayer(
            _context.Process.Type,
            Math.Max(pet.Slot - 5, 0),
            pet.IsMyself
        );

        var damageViewModel = new DamageBarViewModel(playerColor);
        _viewModel.Pets.Damages.Add(damageViewModel);
        _pets.Add(pet, damageViewModel);

        _viewModel.HasPetsToBeDisplayed = _pets.Keys.Count > 0;
    }

    private void RemovePet(IPartyMember pet)
    {
        if (!_pets.ContainsKey(pet))
            return;

        _ = _pets.Remove(pet);
        _viewModel.HasPetsToBeDisplayed = _pets.Keys.Count > 0;
    }

    private void AddPlayer(IPartyMember member)
    {
        if (_members.ContainsKey(member))
            return;

        ObservableColor playerColor = PlayerConfigHelper.GetColorFromPlayer(
            _context.Process.Type,
            member.Slot,
            member.IsMyself
        );

        var memberInfo = new MemberInfo
        {
            ViewModel = new(_view.Settings)
            {
                Name = member.Name,
                Damage = member.Damage,
                Weapon = member.Weapon,
                Bar = new(playerColor),
                IsUser = member.IsMyself,
                MasterRank = member.MasterRank
            },
            Series = BuildPlayerSeries(member.Name, playerColor),
            JoinedAt = _context.Game.TimeElapsed
        };

        _members.Add(member, memberInfo);

        member.OnDamageDealt += OnDamageDealt;
        member.OnWeaponChange += OnWeaponChange;

        PlayerViewModel model = _members[member].ViewModel;

        _logger.Debug($"Added player: {member.GetHashCode():X} {member.Name} with joinedAt: {memberInfo.JoinedAt}");

        _viewModel.Players.Add(model);
    }

    private void RemovePlayer(IPartyMember member)
    {
        if (!_members.ContainsKey(member))
            return;

        member.OnDamageDealt -= OnDamageDealt;
        member.OnWeaponChange -= OnWeaponChange;

        _ = _viewModel.Players.Remove(_members[member].ViewModel);
        _ = _viewModel.Series.Remove(_members[member].Series);
        _ = _members.Remove(member);

        _logger.Debug($"Removed player {member.GetHashCode():X}: {member.Name}");
    }

    private double CalculateDpsByConfiguredStrategy(MemberInfo member)
    {
        double timeElapsed = _view.Settings.DpsCalculationStrategy.Value switch
        {
            DPSCalculationStrategy.RelativeToQuest => _viewModel.TimeElapsed,
            DPSCalculationStrategy.RelativeToJoin => _viewModel.TimeElapsed - Math.Min(_viewModel.TimeElapsed, member.JoinedAt),
            DPSCalculationStrategy.RelativeToFirstHit => _viewModel.TimeElapsed - Math.Min(_viewModel.TimeElapsed, member.FirstHitAt),
            _ => 1,
        };
        timeElapsed = Math.Max(1, timeElapsed);

        return member.ViewModel.Damage / timeElapsed;
    }

    private ObservablePoint CalculatePointByConfiguredStrategy(PlayerViewModel player)
    {
        double damage = _view.Settings.DamagePlotStrategy.Value switch
        {
            DamagePlotStrategy.TotalDamage => player.Damage,
            DamagePlotStrategy.DamagePerSecond => player.DPS,
            _ => throw new NotImplementedException(),
        };

        return new ObservablePoint(_viewModel.TimeElapsed, damage);
    }
    #endregion
}