using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using System;
using System.Linq;

namespace HunterPie.UI.Controls.Settings.Monsters.Builders;

public static class MonsterGlobalConfigurationViewModelBuilder
{
    public static MonsterGlobalConfigurationViewModel Build(
        GameProcessType game,
        ObservableHashSet<PartGroupType> partGroups,
        ObservableHashSet<int> allowedAilments
    )
    {
        GameType? gameType = MapFactory.Map<GameProcessType, GameType?>(game);

        if (gameType is not { })
            throw new ArgumentNullException($"{nameof(game)} must be a valid game");

        AilmentDefinition[] ailmentDefinitions = MonsterAilmentRepository.FindAllBy(gameType.Value);

        PartGroupType[] allPartGroups = Enum.GetValues<PartGroupType>();

        return new MonsterGlobalConfigurationViewModel(
            parts: allPartGroups.Select(it => new MonsterPartGroupViewModel(it, partGroups)
            {
                IsEnabled = partGroups.Contains(it)
            }).ToObservableCollection(),
            ailments: ailmentDefinitions.Select(it => new MonsterGlobalAilmentViewModel(allowedAilments, it.Id, it.String)
            {
                IsEnabled = allowedAilments.Contains(it.Id)
            }).ToObservableCollection()
        );
    }
}