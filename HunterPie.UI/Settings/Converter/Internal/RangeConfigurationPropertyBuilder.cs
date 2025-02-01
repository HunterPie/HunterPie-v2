using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels.Internal;
using System;
using Range = HunterPie.Core.Settings.Types.Range;

namespace HunterPie.UI.Settings.Converter.Internal;

#nullable enable
internal class RangeConfigurationPropertyBuilder : IConfigurationPropertyBuilder
{
    public IConfigurationProperty Build(PropertyData data, GameProcessType game)
    {
        if (data.Value is not Range value)
            throw new ArgumentException($"Property must be of type {nameof(Range)}");

        return new RangePropertyViewModel(value)
        {
            Name = data.Name,
            Description = data.Description,
            Group = data.Group,
            RequiresRestart = data.RequiresRestart,
            Condition = data.Condition,
        };
    }
}