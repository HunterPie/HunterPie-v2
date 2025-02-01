using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Navigation;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels.Internal;
using System;

namespace HunterPie.UI.Settings.Converter.Internal;

internal class AbnormalityTrayConfigurationPropertyBuilder : IConfigurationPropertyBuilder
{
    private readonly ConfigurationAdapter _configurationAdapter;
    private readonly IBodyNavigator _bodyNavigator;

    public AbnormalityTrayConfigurationPropertyBuilder(
        ConfigurationAdapter configurationAdapter,
        IBodyNavigator bodyNavigator)
    {
        _configurationAdapter = configurationAdapter;
        _bodyNavigator = bodyNavigator;
    }

    public IConfigurationProperty Build(PropertyData data, GameProcessType game)
    {
        if (data.Value is not AbnormalityTrays value)
            throw new ArgumentException($"Property must be of type {nameof(AbnormalityTrays)}");

        return new AbnormalityTrayPropertyViewModel(
            trays: value.Trays,
            game: game,
            configurationAdapter: _configurationAdapter,
            bodyNavigator: _bodyNavigator)
        {
            Name = data.Name,
            Description = data.Description,
            Group = data.Group,
            RequiresRestart = data.RequiresRestart,
            Condition = data.Condition,
        };
    }
}