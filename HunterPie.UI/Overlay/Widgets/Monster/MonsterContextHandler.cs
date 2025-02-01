using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services.Monster.Events;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enemy;
using HunterPie.UI.Overlay.Widgets.Monster.Adapters;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Monster;

public class MonsterContextHandler : BossMonsterViewModel, IContextHandler, IDisposable
{
    private readonly IGame _game;
    public readonly IMonster Context;

    public MonsterContextHandler(
        IGame game,
        IMonster context,
        MonsterWidgetConfig config
    ) : base(config)
    {
        _game = game;
        Context = context;
        HookEvents();

        AddEnrage();
        UpdateData();
    }

    public void HookEvents()
    {
        _game.TargetDetectionService.OnTargetChanged += OnTargetDetectionChanged;
        Config.TargetMode.PropertyChanged += OnTargetModeChange;
        Context.OnHealthChange += OnHealthUpdate;
        Context.OnStaminaChange += OnStaminaUpdate;
        Context.OnCaptureThresholdChange += OnCaptureThresholdChange;
        Context.OnEnrageStateChange += OnEnrageStateChange;
        Context.OnSpawn += OnSpawn;
        Context.OnDeath += OnDespawn;
        Context.OnDespawn += OnDespawn;
        Context.OnTargetChange += OnTargetChange;
        Context.OnNewPartFound += OnNewPartFound;
        Context.OnNewAilmentFound += OnNewAilmentFound;
        Context.OnCrownChange += OnCrownChange;
        Context.OnWeaknessesChange += OnWeaknessesChange;
    }

    public void UnhookEvents()
    {
        _game.TargetDetectionService.OnTargetChanged -= OnTargetDetectionChanged;
        Config.TargetMode.PropertyChanged -= OnTargetModeChange;
        Context.OnHealthChange -= OnHealthUpdate;
        Context.OnStaminaChange -= OnStaminaUpdate;
        Context.OnCaptureThresholdChange -= OnCaptureThresholdChange;
        Context.OnEnrageStateChange -= OnEnrageStateChange;
        Context.OnSpawn -= OnSpawn;
        Context.OnDeath -= OnDespawn;
        Context.OnDespawn -= OnDespawn;
        Context.OnTargetChange -= OnTargetChange;
        Context.OnNewPartFound -= OnNewPartFound;
        Context.OnNewAilmentFound -= OnNewAilmentFound;
        Context.OnCrownChange -= OnCrownChange;
        Context.OnWeaknessesChange -= OnWeaknessesChange;
    }

    private void OnTargetDetectionChanged(object sender, InferTargetChangedEventArgs e) =>
        HandleTargetUpdate(
            lockOnTarget: Context.Target,
            manualTarget: Context.ManualTarget,
            inferredTarget: _game.TargetDetectionService.Infer(Context)
        );

    private void OnTargetModeChange(object sender, PropertyChangedEventArgs _) =>
        HandleTargetUpdate(
            lockOnTarget: Context.Target,
            manualTarget: Context.ManualTarget,
            inferredTarget: _game.TargetDetectionService.Infer(Context)
        );

    private void OnSpawn(object sender, EventArgs e)
    {
        IsQurio = Context is MHRMonster { MonsterType: MonsterType.Qurio };
        Name = Context.Name;

        Em = BuildMonsterEmByContext();

        IsAlive = true;

        FetchMonsterIcon();

        UIThread.BeginInvoke(() =>
        {
            if (Types.Count > 0)
                return;

            foreach (string typeId in Context.Types)
                Types.Add(typeId);
        });
    }

    private void OnDespawn(object sender, EventArgs e)
    {
        IsAlive = false;
    }

    private void OnCaptureThresholdChange(object sender, IMonster e)
    {
        CaptureThreshold = e.CaptureThreshold;
        IsCapturable = CaptureThreshold >= (Health / MaxHealth);
        CanBeCaptured = e.CaptureThreshold > 0;
    }
    private void OnWeaknessesChange(object sender, Element[] e)
    {
        UIThread.BeginInvoke(() =>
        {
            lock (Weaknesses)
            {
                Weaknesses.Clear();

                foreach (Element weakness in e)
                    Weaknesses.Add(weakness);
            }
        });
    }

    private void OnStaminaUpdate(object sender, EventArgs e)
    {
        MaxStamina = Context.MaxStamina;
        Stamina = Context.Stamina;
    }

    private void OnEnrageStateChange(object sender, EventArgs e) => IsEnraged = Context.IsEnraged;

    private void OnCrownChange(object sender, EventArgs e) => Crown = Context.Crown;

    private void OnNewAilmentFound(object sender, IMonsterAilment e)
    {
        UIThread.BeginInvoke(() =>
        {
            bool contains = Ailments.ToArray()
                        .Cast<MonsterAilmentContextHandler>()
                        .Any(p => p.Context == e);

            if (contains)
                return;

            Ailments.Add(new MonsterAilmentContextHandler(Context, e, Config));
        });
    }

    private void OnNewPartFound(object sender, IMonsterPart e)
    {
        UIThread.Invoke(() =>
        {
            bool contains = Parts.ToArray()
                        .Cast<MonsterPartContextHandler>()
                        .Any(p => p.Context == e);

            if (contains)
                return;

            Parts.Add(new MonsterPartContextHandler(Context, e, Config));
        });
    }

    private void OnTargetChange(object sender, MonsterTargetEventArgs e) =>
        HandleTargetUpdate(
            lockOnTarget: e.LockOnTarget,
            manualTarget: e.ManualTarget,
            inferredTarget: _game.TargetDetectionService.Infer(Context)
        );

    private void OnHealthUpdate(object sender, EventArgs e)
    {
        MaxHealth = Context.MaxHealth;
        Health = Context.Health;
        IsCapturable = CaptureThreshold >= (Health / MaxHealth);
    }

    private void UpdateData()
    {
        IsQurio = Context is MHRMonster { MonsterType: MonsterType.Qurio };

        if (Context.Id > -1)
        {
            Name = Context.Name;
            Em = BuildMonsterEmByContext();

            FetchMonsterIcon();
        }

        MaxHealth = Context.MaxHealth;
        Health = Context.Health;
        HandleTargetUpdate(
            lockOnTarget: Context.Target,
            manualTarget: Context.ManualTarget,
            inferredTarget: _game.TargetDetectionService.Infer(Context)
        );
        MaxStamina = Context.MaxStamina;
        Stamina = Context.Stamina;
        TargetType = Context.Target;
        Crown = Context.Crown;
        IsEnraged = Context.IsEnraged;
        IsAlive = Context.Health > 0;
        CaptureThreshold = Context.CaptureThreshold;
        CanBeCaptured = Context.CaptureThreshold > 0;
        IsCapturable = CaptureThreshold >= (Health / MaxHealth);

        UIThread.BeginInvoke(() =>
        {
            foreach (string typeId in Context.Types)
                Types.Add(typeId);
        });

        if (Parts.Count != Context.Parts.Length || Ailments.Count != Context.Ailments.Count)
            UIThread.BeginInvoke(() =>
            {
                foreach (IMonsterPart part in Context.Parts)
                {
                    bool contains = Parts
                        .ToArray()
                        .Cast<MonsterPartContextHandler>()
                        .Any(p => p.Context == part);

                    if (contains)
                        continue;

                    Parts.Add(new MonsterPartContextHandler(Context, part, Config));
                }

                foreach (IMonsterAilment ailment in Context.Ailments)
                {
                    bool contains = Ailments
                        .ToArray()
                        .Cast<MonsterAilmentContextHandler>()
                        .Any(p => p.Context == ailment);

                    if (contains)
                        continue;

                    Ailments.Add(new MonsterAilmentContextHandler(Context, ailment, Config));
                }

                foreach (Element weakness in Context.Weaknesses)
                    Weaknesses.Add(weakness);
            });
    }

    private void AddEnrage() => UIThread.BeginInvoke(() => Ailments.Add(new MonsterAilmentContextHandler(Context, Context.Enrage, Config)));

    private string BuildMonsterEmByContext()
    {
        return Context switch
        {
            MHRMonster ctx => $"Rise_{ctx.Id:00}",
            MHWMonster ctx => $"World_{ctx.Id:00}",
            _ => throw new NotImplementedException("unreachable")
        };
    }

    private void HandleTargetUpdate(
        Target lockOnTarget,
        Target manualTarget,
        Target inferredTarget
    )
    {
        TargetType = MonsterTargetAdapter.Adapt(Config, lockOnTarget, manualTarget, inferredTarget);
        IsTarget = TargetType == Target.Self || (TargetType == Target.None && !Config.ShowOnlyTarget);
    }

    public void Dispose()
    {
        UnhookEvents();

        foreach (MonsterPartViewModel part in Parts)
            part.Dispose();

        foreach (MonsterAilmentViewModel ailment in Ailments)
            ailment.Dispose();

        Parts.Clear();
        Ailments.Clear();
    }
}