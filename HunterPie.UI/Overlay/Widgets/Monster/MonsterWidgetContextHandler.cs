using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Overlay.Widgets.Monster.Adapters;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using System;
using System.ComponentModel;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Monster;

public class MonsterWidgetContextHandler : IContextHandler
{
    private readonly MonstersViewModel _viewModel;
    private readonly MonstersView _view;
    private MonsterWidgetConfig Settings => _view.Settings;
    private readonly IContext _context;
    private readonly MonsterWidgetConfig _config;

    public MonsterWidgetContextHandler(IContext context)
    {
        _config = ClientConfigHelper.DeferOverlayConfig(
            game: context.Process.Type,
            (config) => config.BossesWidget
        );

        _view = new MonstersView(_config);
        _ = WidgetManager.Register<MonstersView, MonsterWidgetConfig>(_view);

        _viewModel = _view.ViewModel;
        _context = context;

        UpdateData();
        HookEvents();
    }

    private void UpdateData()
    {
        foreach (IMonster monster in _context.Game.Monsters)
        {
            monster.OnTargetChange += OnTargetChange;
            _viewModel.Monsters.Add(new MonsterContextHandler(_context.Game, monster, Settings));
        }

        CalculateVisibleMonsters();
    }

    public void HookEvents()
    {
        _config.TargetMode.PropertyChanged += OnTargetModeChanged;
        _context.Game.OnMonsterSpawn += OnMonsterSpawn;
        _context.Game.OnMonsterDespawn += OnMonsterDespawn;
    }

    public void UnhookEvents()
    {
        _config.TargetMode.PropertyChanged -= OnTargetModeChanged;
        _context.Game.OnMonsterSpawn -= OnMonsterSpawn;
        _context.Game.OnMonsterDespawn -= OnMonsterDespawn;

        _view.Dispatcher.BeginInvoke(() =>
        {
            foreach (MonsterContextHandler ctxHandler in _viewModel.Monsters.Cast<MonsterContextHandler>())
                ctxHandler.Dispose();

            _viewModel.Monsters.Clear();
        });

        _ = WidgetManager.Unregister<MonstersView, MonsterWidgetConfig>(_view);
    }

    private void OnTargetModeChanged(object _, PropertyChangedEventArgs __) => CalculateVisibleMonsters();

    private void OnMonsterDespawn(object sender, IMonster e)
    {
        _view.Dispatcher.BeginInvoke(() =>
        {
            MonsterContextHandler monster = _viewModel.Monsters
                .Cast<MonsterContextHandler>()
                .FirstOrDefault(handler => handler.Context == e);

            if (monster is null)
                return;

            monster.Dispose();

            _ = _viewModel.Monsters.Remove(monster);
        });

        e.OnTargetChange -= OnTargetChange;
        CalculateVisibleMonsters();
    }

    private void OnMonsterSpawn(object sender, IMonster monster)
    {
        _view.Dispatcher.BeginInvoke(() => _viewModel.Monsters.Add(new MonsterContextHandler(_context.Game, monster, Settings)));

        monster.OnTargetChange += OnTargetChange;
        CalculateVisibleMonsters();
    }

    private void OnTargetChange(object sender, EventArgs e) => CalculateVisibleMonsters();

    private void CalculateVisibleMonsters()
    {
        int targets = _context.Game.Monsters.Count(monster =>
        {
            Target target = MonsterTargetAdapter.Adapt(
                config: _config,
                lockOnTarget: monster.Target,
                manualTarget: monster.ManualTarget,
                inferredTarget: _context.Game.TargetDetectionService.Infer(monster)
            );
            return target == Target.Self;
        });

        _viewModel.VisibleMonsters = Settings.ShowOnlyTarget.Value switch
        {
            true => targets,
            false => targets == 0 ? _context.Game.Monsters.Count : targets,
        };
        _viewModel.MonstersCount = _context.Game.Monsters.Count;
    }
}