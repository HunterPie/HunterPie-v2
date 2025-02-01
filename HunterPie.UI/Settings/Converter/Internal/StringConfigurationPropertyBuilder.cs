using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels.Internal;
using System;

namespace HunterPie.UI.Settings.Converter.Internal;

#nullable enable
internal class StringConfigurationPropertyBuilder : IConfigurationPropertyBuilder
{
    public IConfigurationProperty Build(PropertyData data, GameProcessType game)
    {
        if (data.Value is not Observable<string> value)
            throw new ArgumentException($"Property must be of type {nameof(Observable<string>)}");

        return new StringPropertyViewModel(value)
        {
            Name = data.Name,
            Description = data.Description,
            Group = data.Group,
            RequiresRestart = data.RequiresRestart,
            Condition = data.Condition,
        };
    }
}