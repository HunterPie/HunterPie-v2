using HunterPie.UI.Architecture;
using HunterPie.UI.Settings.Models;

namespace HunterPie.Features.Settings.ViewModels;

internal class ConfigurationCategoryTab : ViewModel, IConfigurationCategory
{
    public required ConfigurationCategory Category { get; init; }
}