using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;

namespace HunterPie.UI.Settings.Converter;

#nullable enable
public interface IConfigurationPropertyBuilder
{
    public IConfigurationProperty Build(PropertyData data, GameProcessType game);
}