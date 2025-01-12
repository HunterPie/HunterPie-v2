using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System.Collections.Specialized;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Monster;

#nullable enable
public class MonsterAilmentContextHandler : MonsterAilmentViewModel
{
    private MonsterConfiguration? _monsterConfiguration;
    private readonly MonsterDetailsConfiguration _detailsConfiguration;
    private readonly IMonster _monsterContext;

    public readonly IMonsterAilment Context;

    public MonsterAilmentContextHandler(
        IMonster monsterContext,
        IMonsterAilment context,
        MonsterWidgetConfig config
    ) : base(config)
    {
        _monsterContext = monsterContext;
        _detailsConfiguration = config.Details;
        Context = context;

        HookEvents();
        Update();
    }

    private void HookEvents()
    {
        Context.OnTimerUpdate += OnTimerUpdate;
        Context.OnBuildUpUpdate += OnBuildUpUpdate;
        Context.OnCounterUpdate += OnCounterUpdate;
        _detailsConfiguration.AllowedAilments.CollectionChanged += OnAllowedAilmentsChanged;
        _detailsConfiguration.Monsters.CollectionChanged += OnMonsterConfigurationsChanged;
        HandleMonsterConfiguration();
    }

    private void UnhookEvents()
    {
        Context.OnTimerUpdate -= OnTimerUpdate;
        Context.OnBuildUpUpdate -= OnBuildUpUpdate;
        Context.OnCounterUpdate -= OnCounterUpdate;
        _detailsConfiguration.AllowedAilments.CollectionChanged -= OnAllowedAilmentsChanged;
        _detailsConfiguration.Monsters.CollectionChanged -= OnMonsterConfigurationsChanged;
        if (_monsterConfiguration is { })
            _monsterConfiguration.Ailments.CollectionChanged -= OnMonsterConfigurationAilmentsChanged;
    }

    private void OnMonsterConfigurationsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => HandleMonsterConfiguration();

    private void HandleMonsterConfiguration()
    {
        MonsterConfiguration? specificConfiguration = _detailsConfiguration.Monsters.FirstOrDefault(it => it.Id == _monsterContext.Id);

        switch (specificConfiguration)
        {
            case null when _monsterConfiguration is null:
                return;

            case null when _monsterConfiguration is not null:
                _monsterConfiguration.Ailments.CollectionChanged -= OnMonsterConfigurationAilmentsChanged;
                _monsterConfiguration = null;
                return;

            case not null when _monsterConfiguration is null:
                _monsterConfiguration = specificConfiguration;
                _monsterConfiguration.Ailments.CollectionChanged += OnMonsterConfigurationAilmentsChanged;
                return;
        }

        HandleEnabledState();
    }

    private void OnMonsterConfigurationAilmentsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => HandleEnabledState();

    private void OnAllowedAilmentsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => HandleEnabledState();

    private void OnCounterUpdate(object? sender, IMonsterAilment e) => Count = e.Counter;

    private void OnBuildUpUpdate(object? sender, IMonsterAilment e)
    {
        if (e.MaxBuildUp <= 0)
            return;

        MaxBuildup = e.MaxBuildUp;
        Buildup = e.BuildUp;
    }

    private void OnTimerUpdate(object? sender, IMonsterAilment e)
    {
        if (e.MaxTimer <= 0)
            return;

        MaxTimer = e.MaxTimer;
        Timer = e.Timer;
    }

    private void Update()
    {
        Name = Context.Id;
        Count = Context.Counter;
        MaxBuildup = Context.MaxBuildUp;
        Buildup = Context.BuildUp;
        MaxTimer = Context.MaxTimer;
        Timer = Context.Timer;
        HandleEnabledState();
    }

    private void HandleEnabledState()
    {
        bool isGloballyEnabled = _detailsConfiguration.AllowedAilments.Contains(Context.Definition.Id);
        bool? isSpecificallyEnabled = _monsterConfiguration?.Ailments.FirstOrDefault(it => it.Id == Context.Definition.Id)
            ?.IsEnabled.Value;
        IsEnabled = isSpecificallyEnabled switch
        {
            { } enabled => enabled,
            _ => isGloballyEnabled
        };
    }

    public override void Dispose()
    {
        UnhookEvents();
        base.Dispose();
    }
}