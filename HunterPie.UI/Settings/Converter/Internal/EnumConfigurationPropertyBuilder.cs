using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Adapters;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels.Internal;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Settings.Converter.Internal;

#nullable enable
internal class EnumConfigurationPropertyBuilder : IConfigurationPropertyBuilder
{
    public IConfigurationProperty Build(PropertyData data, GameProcessType game)
    {
        Type observableType = typeof(Observable<>);
        Type innerType = data.Value.GetType();

        if (!(innerType.GetGenericTypeDefinition() == observableType))
            throw new ArgumentException("Value must be an Observable");

        Type? enumType = innerType.GenericTypeArguments.FirstOrDefault();

        if (enumType is null || !enumType.IsEnum)
            throw new ArgumentException("Value must be an Enum");

        object[] enumValues = (data.Adapter) switch
        {
            IEnumAdapter adapter => adapter.GetValues(game),
            _ => Enum.GetValues(enumType)
                     .Cast<object>()
                     .ToArray()
        };
        ObservableCollection<object> elements = new(enumValues);

        return new EnumPropertyViewModel(data.Value, elements)
        {
            Name = data.Name,
            Description = data.Description,
            Group = data.Group,
            RequiresRestart = data.RequiresRestart,
            Condition = data.Condition,
        };
    }
}