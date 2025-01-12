using HunterPie.Core.Client.Localization;
using HunterPie.Core.Extensions;
using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.Features.Statistics.Models;
using HunterPie.UI.Architecture.Adapter;
using HunterPie.UI.Assets.Application;
using LiveCharts.Wpf;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HunterPie.Features.Statistics.Details.Builders;
internal static class MonsterDetailsViewModelBuilder
{
    private static readonly Brush EnrageBrush = Resources.Get<Brush>("HUNT_EXPORT_ENRAGE_BRUSH");

    public static async Task<MonsterDetailsViewModel> Build(HuntStatisticsModel hunt, MonsterModel monster)
    {
        TimeSpan? huntElapsed = null;
        if (monster.HuntStartedAt is { } startedAt && monster.HuntFinishedAt is { } finishedAt)
            huntElapsed = finishedAt - startedAt;

        return new MonsterDetailsViewModel
        {
            Name = MonsterNameAdapter.From(hunt.Game, monster.Id),
            Icon = await MonsterIconAdapter.UriFrom(hunt.Game, monster.Id),
            TimeElapsed = hunt.FinishedAt - hunt.StartedAt,
            MaxHealth = monster.MaxHealth,
            HuntElapsed = huntElapsed,
            Statuses = { BuildEnrage(hunt, huntElapsed ?? TimeSpan.Zero, monster.Enrage) },
            Players = hunt.Players.Select(it => PartyMemberDetailsViewModelBuilder.Build(hunt, monster, it))
                                  .FilterNull()
                                  .ToObservableCollection(),
            Crown = monster.Crown,
        };
    }

    private static StatusDetailsViewModel BuildEnrage(
        HuntStatisticsModel quest,
        TimeSpan huntTimeElapsed,
        MonsterStatusModel status
    )
    {
        double timeElapsed = huntTimeElapsed.TotalSeconds;

        double activationsTotalSeconds = status.Activations.Select(it => it.FinishedAt - it.StartedAt)
            .Sum(it => it.TotalSeconds);

        var activations = status.Activations.Select(it => new AxisSection
        {
            StrokeThickness = 1,
            Stroke = new SolidColorBrush(Colors.Red) { Opacity = 0.15 },
            Fill = new SolidColorBrush(Colors.Red) { Opacity = 0.05 },
            StrokeDashArray = new DoubleCollection { 4, 4 },
            Value = (it.StartedAt - quest.StartedAt).TotalSeconds,
            SectionWidth = (it.FinishedAt - it.StartedAt).TotalSeconds
        }).ToList();

        return new StatusDetailsViewModel
        {
            Color = EnrageBrush,
            Name = Localization.QueryString("//Strings/Ailments/Rise/Ailment[@Id='STATUS_ENRAGE']"),
            UpTime = activationsTotalSeconds / Math.Max(1.0, timeElapsed),
            Activations = activations
        };
    }
}