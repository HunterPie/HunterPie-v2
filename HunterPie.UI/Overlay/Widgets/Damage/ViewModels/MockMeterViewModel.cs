using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture.Graphs;
using HunterPie.UI.Architecture.Test;
using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class MockMeterViewModel : MeterViewModel
{
    private int totalDamage = 0;
    private static readonly DamageMeterWidgetConfig _mockConfig = new();
    public readonly List<ChartValues<ObservablePoint>> _playerChartValues = new();
    public readonly double[] _petDamages = new double[4];

    public MockMeterViewModel() : base(_mockConfig)
    {
        InHuntingZone = true;
        HasPetsToBeDisplayed = true;

        MockPlayers();
        MockPlayerSeries();
        MockPets();

        MockBehavior.Run(() => MockInGameAction(), (float)TimeSpan.FromMilliseconds(16).TotalSeconds);
    }

    private void MockPets()
    {
        Pets.Name = "Otomos";
        Pets.Damages.Add(new(_mockConfig.PlayerFirst));
        Pets.Damages.Add(new(_mockConfig.PlayerSelf));
        Pets.Damages.Add(new(_mockConfig.PlayerThird));
        Pets.Damages.Add(new(_mockConfig.PlayerFourth));
    }

    private void MockPlayers()
    {
        Players.Add(new(_mockConfig)
        {
            Name = "Player 1",
            Weapon = Weapon.Bow,
            Bar = new(_mockConfig.PlayerFirst),
        });
        Players.Add(new(_mockConfig)
        {
            Name = "Player 2",
            Weapon = Weapon.ChargeBlade,
            Bar = new(_mockConfig.PlayerSelf),
            IsUser = true
        });
        Players.Add(new(_mockConfig)
        {
            Name = "Player 3",
            Weapon = Weapon.Greatsword,
            Bar = new(_mockConfig.PlayerThird),
        });
        Players.Add(new(_mockConfig)
        {
            Name = "Player 4",
            Weapon = Weapon.HuntingHorn,
            Bar = new(_mockConfig.PlayerFourth),
        });
    }

    private void MockInGameAction()
    {
        Random random = new();
        int i = 1;
        double newTime = TimeElapsed + 0.016;

        if ((int)newTime > (int)TimeElapsed)
        {
            Pets.TotalDamage = random.Next(0, 10000);

            foreach (PlayerViewModel player in Players)
            {
                double lastDps = player.DPS;
                int hit = random.Next(0, random.Next(1, 20));
                bool shouldHit = hit % 2 == 1;
                player.Damage += hit;
                player.DPS = player.Damage / TimeElapsed;
                player.Bar.Percentage = player.Damage / (double)Math.Max(1, totalDamage) * 100;
                player.IsIncreasing = lastDps < player.DPS;

                _playerChartValues[i - 1].Add(new ObservablePoint(TimeElapsed, player.DPS));
                totalDamage += hit;
                i++;
            }

            double[] petDamages = { random.NextDouble() * 100, random.NextDouble() * 100, random.NextDouble() * 100, random.NextDouble() * 100, };
            Pets.TotalDamage = (int)(_petDamages.Sum() + petDamages.Sum());

            i = 0;
            foreach (DamageBarViewModel pet in Pets.Damages)
            {
                double petDamage = petDamages[i];
                _petDamages[i] += petDamage;
                pet.Percentage = _petDamages[i] / Pets.TotalDamage * 100;
                i++;
            }

            Application.Current.Dispatcher.Invoke(SortMembers);
        }

        TimeElapsed = newTime;
    }

    private void MockPlayerSeries()
    {
        int i = 0;
        LinearSeriesCollectionBuilder builder = new();
        foreach (PlayerViewModel player in Players)
        {
            _playerChartValues.Add(new());
            var color = (Color)ColorConverter.ConvertFromString(player.Bar.Color);
            _ = builder.AddSeries(_playerChartValues[i], player.Name, color);
            i++;
        }

        Series = builder.Build();
    }
}