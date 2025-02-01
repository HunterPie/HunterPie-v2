using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.UI.Architecture.Adapter;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.UI.Controls.Settings.Monsters.Builders;

#nullable enable
public static class MonsterPartsViewModelBuilder
{
    public static MonsterConfigurationViewModel[] Build(
        GameProcessType game,
        ICollection<MonsterConfiguration> configurations
    )
    {
        GameType? gameType = MapFactory.Map<GameProcessType, GameType?>(game);

        if (gameType is not { })
            return Array.Empty<MonsterConfigurationViewModel>();

        var monsterConfigurations = configurations.ToDictionary(it => it.Id);
        AilmentDefinition[] ailmentDefinitions = MonsterAilmentRepository.FindAllBy(gameType.Value);

        IEnumerable<MonsterConfigurationViewModel> viewModels = MonsterRepository.FindAllBy(gameType.Value)
            .Select(it =>
            {
                MonsterConfiguration? configuration = monsterConfigurations.GetValueOrDefault(it.Id);
                var partConfigurations = configuration?.Parts.DistinctBy(it => it.Id)
                                                                                           .ToDictionary(part => part.Id);
                var ailmentConfigurations = configuration?.Ailments.ToDictionary(ailment => ailment.Id);

                IEnumerable<MonsterPartConfiguration> parts = it.Parts.Select(part =>
                {
                    MonsterPartConfiguration? partConfiguration = partConfigurations?.GetValueOrDefault(part.Id);

                    var newPartConfig = new MonsterPartConfiguration
                    {
                        Id = part.Id,
                        StringId = part.String,
                        IsEnabled = partConfiguration?.IsEnabled ?? new Observable<bool>(true)
                    };

                    // We need to inject a new configuration if a monster is already being overriden
                    // and we don't have a part configuration
                    if (partConfiguration is not { } && configuration is { })
                        configuration.Parts.Add(newPartConfig);


                    return newPartConfig;
                });

                IEnumerable<MonsterAilmentConfiguration> ailments = ailmentDefinitions.Select(ailment =>
                {
                    MonsterAilmentConfiguration? ailmentConfiguration =
                        ailmentConfigurations?.GetValueOrDefault(ailment.Id);

                    return new MonsterAilmentConfiguration
                    {
                        Id = ailment.Id,
                        StringId = ailment.String,
                        IsEnabled = ailmentConfiguration?.IsEnabled ?? new Observable<bool>(true)
                    };
                });

                return new MonsterConfigurationViewModel
                {
                    Name = MonsterNameAdapter.From(gameType.Value, it.Id),
                    GameType = gameType.Value,
                    Configuration = new MonsterConfiguration
                    {
                        Id = it.Id,
                        Parts = parts.ToObservableCollection(),
                        Ailments = ailments.ToObservableCollection(),
                    },
                    IsOverriding = configuration is not null
                };
            });

        return viewModels.ToArray();
    }
}