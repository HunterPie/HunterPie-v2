using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.UI.Controls.Settings.Abnormality.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Controls.Settings.Abnormality.Builders;

#nullable enable
public static class AbnormalityCategoryViewModelBuilder
{
    private const string CATEGORY_PATH = "//Strings/Client/Settings/Setting[@Id='ABNORMALITY_{0}_STRING']";
    private const string ABNORMALITY_XPATH = "//Strings/Abnormalities/Abnormality[@Id='{0}']";
    private const string ICON_SONGS = "ICON_SELFIMPROVEMENT";
    private const string ICON_PALICO = "ICON_ORCHESTRA";
    private const string ICON_CONSUMABLES = "ITEM_DEMONDRUG";
    private const string ICON_DEBUFFS = "ICON_VENOM";
    private const string ICON_SKILLS = "ICON_BUILD";
    private const string ICON_FOODS = "ICON_DANGO";
    private static readonly ObservableCollection<AbnormalityCategoryViewModel> EmptyCached = new();

    public static ObservableCollection<AbnormalityCategoryViewModel> Build(GameProcessType game)
    {
        GameType? gameType = MapFactory.Map<GameProcessType, GameType?>(game);

        if (gameType is null)
            return EmptyCached;

        IEnumerable<IGrouping<string, AbnormalityDefinition>> abnormalities = AbnormalityRepository.FindAllAbnormalitiesBy(gameType.Value);

        return abnormalities.Select(group =>
        {
            (string categoryName, string categoryDescription) =
                Localization.Resolve(CATEGORY_PATH.Format(group.Key.ToUpperInvariant()));

            return new AbnormalityCategoryViewModel
            {
                Name = categoryName,
                Description = categoryDescription,
                Elements = group.Select(element =>
                    new AbnormalityElementViewModel
                    {
                        Id = element.Id,
                        Name = Localization.QueryString(ABNORMALITY_XPATH.Format(element.Name)),
                        Category = categoryName,
                        Icon = element.Icon,
                        IsMatch = true
                    }
                ).ToObservableCollection()
            };
        }).ToObservableCollection();
    }
}