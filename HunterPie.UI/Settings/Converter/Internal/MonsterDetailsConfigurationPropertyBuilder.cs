using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels.Internal;
using System;

namespace HunterPie.UI.Settings.Converter.Internal;

public class MonsterDetailsConfigurationPropertyBuilder : IConfigurationPropertyBuilder
{
    public IConfigurationProperty Build(PropertyData data, GameProcessType game)
    {
        if (data.Value is not MonsterDetailsConfiguration value)
            throw new ArgumentException($"Property must be of type {nameof(MonsterDetailsConfiguration)}");

        return new MonsterDetailsPropertyViewModel
        {
            Configurations = value.Monsters,
            Game = game,
            Name = data.Name,
            Description = data.Description,
            Group = data.Group,
            RequiresRestart = data.RequiresRestart,
            Condition = data.Condition
        };
    }
}