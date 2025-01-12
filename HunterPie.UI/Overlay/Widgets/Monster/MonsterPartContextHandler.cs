using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Settings.Types;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System.Collections.Specialized;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Monster;

#nullable enable
public class MonsterPartContextHandler : MonsterPartViewModel
{
    private MonsterConfiguration? _monsterConfiguration;
    private readonly MonsterDetailsConfiguration _detailsConfiguration;
    private readonly IMonster _monsterContext;

    public readonly IMonsterPart Context;

    public MonsterPartContextHandler(
        IMonster monsterContext,
        IMonsterPart context,
        MonsterWidgetConfig config
    ) : base(config)
    {
        _monsterContext = monsterContext;
        _detailsConfiguration = config.Details;
        Context = context;
        Type = Context.Type;

        HookEvents();
        Update();
    }

    private void HookEvents()
    {
        Context.OnHealthUpdate += OnHealthUpdate;
        Context.OnFlinchUpdate += OnFlinchUpdate;
        Context.OnTenderizeUpdate += OnTenderizeUpdate;
        Context.OnSeverUpdate += OnSeverUpdate;
        Context.OnBreakCountUpdate += OnBreakCountUpdate;
        Context.OnPartTypeChange += OnPartTypeChange;
        _detailsConfiguration.AllowedPartGroups.CollectionChanged += OnAllowedPartGroupsChanged;
        _detailsConfiguration.Monsters.CollectionChanged += OnMonsterConfigurationsChanged;
        HandleMonsterConfiguration();
        HookMHREvents();
    }

    private void UnhookEvents()
    {
        Context.OnHealthUpdate -= OnHealthUpdate;
        Context.OnFlinchUpdate -= OnFlinchUpdate;
        Context.OnTenderizeUpdate -= OnTenderizeUpdate;
        Context.OnSeverUpdate -= OnSeverUpdate;
        Context.OnBreakCountUpdate -= OnBreakCountUpdate;
        Context.OnPartTypeChange -= OnPartTypeChange;
        _detailsConfiguration.AllowedPartGroups.CollectionChanged -= OnAllowedPartGroupsChanged;
        _detailsConfiguration.Monsters.CollectionChanged -= OnMonsterConfigurationsChanged;
        if (_monsterConfiguration is { })
            _monsterConfiguration.Parts.CollectionChanged -= OnMonsterPartsConfigurationChanged;
        UnhookMHREvents();
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
                _monsterConfiguration.Parts.CollectionChanged -= OnMonsterPartsConfigurationChanged;
                _monsterConfiguration = null;
                return;

            case not null when _monsterConfiguration is null:
                _monsterConfiguration = specificConfiguration;
                _monsterConfiguration.Parts.CollectionChanged += OnMonsterPartsConfigurationChanged;
                return;
        }

        HandleEnabledState();
    }

    private void OnMonsterPartsConfigurationChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => HandleEnabledState();

    private void OnAllowedPartGroupsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        => HandleEnabledState();

    private void OnPartTypeChange(object? sender, IMonsterPart e) => Type = e.Type;

    private void OnSeverUpdate(object? sender, IMonsterPart e)
    {
        MaxSever = e.MaxSever;
        Sever = e.Sever;

        IsPartSevered = MaxSever == Sever && (Breaks > 0 || Flinch != MaxFlinch);
    }

    private void OnTenderizeUpdate(object? sender, IMonsterPart e)
    {
        Tenderize = e.MaxTenderize - e.Tenderize;
        MaxTenderize = e.MaxTenderize;
    }

    private void OnBreakCountUpdate(object? sender, IMonsterPart e) => Breaks = e.Count;

    private void OnFlinchUpdate(object? sender, IMonsterPart e)
    {
        if (Flinch < e.Flinch && MaxFlinch > 0)
            Breaks++;

        MaxFlinch = e.MaxFlinch;
        Flinch = e.Flinch;

        IsPartBroken = MaxHealth <= 0 || (Health == MaxHealth && (Breaks > 0 || Flinch != MaxFlinch));
        IsPartSevered = MaxSever == Sever && (Breaks > 0 || Flinch != MaxFlinch);
    }

    private void OnHealthUpdate(object? sender, IMonsterPart e)
    {
        MaxHealth = e.MaxHealth;
        Health = e.Health;

        IsPartBroken = MaxHealth <= 0 || (Health == MaxHealth && (Breaks > 0 || Flinch != MaxFlinch));
    }

    private void Update()
    {
        Name = Context.Id;
        IsKnownPart = Context.Definition.String != "PART_UNKNOWN";

        MaxHealth = Context.MaxHealth;
        Health = Context.Health;
        MaxFlinch = Context.MaxFlinch;
        Flinch = Context.Flinch;
        MaxSever = Context.MaxSever;
        Sever = Context.Sever;

        IsPartSevered = MaxSever == Sever && (Breaks > 0 || Flinch != MaxFlinch);
        IsPartBroken = MaxHealth <= 0 || (Health == MaxHealth && (Breaks > 0 || Flinch != MaxFlinch));
        IsEnabled = _detailsConfiguration.AllowedPartGroups.Contains(Context.Definition.Group);

        MHRUpdate();
        HandleEnabledState();
    }

    private void MHRUpdate()
    {
        if (Context is not MHRMonsterPart ctx)
            return;

        QurioMaxHealth = ctx.QurioMaxHealth;
        QurioHealth = ctx.QurioHealth;
    }

    private void HandleEnabledState()
    {
        bool isGloballyEnabled = _detailsConfiguration.AllowedPartGroups.Contains(Context.Definition.Group);
        bool? isSpecificallyEnabled = _monsterConfiguration?.Parts.FirstOrDefault(it => it.Id == Context.Definition.Id)
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

    #region Game exclusive hooks 

    private void HookMHREvents()
    {
        if (Context is MHRMonsterPart ctx)
        {
            ctx.OnQurioHealthChange += OnQurioHealthChange;
        }
    }

    private void UnhookMHREvents()
    {
        if (Context is MHRMonsterPart ctx)
        {
            ctx.OnQurioHealthChange -= OnQurioHealthChange;
        }
    }

    private void OnQurioHealthChange(object? sender, IMonsterPart e)
    {
        var part = (MHRMonsterPart)e;

        QurioMaxHealth = part.QurioMaxHealth;
        QurioHealth = part.QurioHealth;
    }

    #endregion
}