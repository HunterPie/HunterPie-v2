using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Controls.Settings.Abnormality.Builders;
using HunterPie.UI.Controls.Settings.Abnormality.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.UI.Settings.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class AbnormalityTrayPropertyViewModel : ConfigurationPropertyViewModel
{
    private readonly ConfigurationAdapter _configurationAdapter;
    private readonly IBodyNavigator _bodyNavigator;

    public ObservableCollection<AbnormalityWidgetConfig> Trays { get; }
    public GameProcessType Game { get; }

    public AbnormalityTrayPropertyViewModel(
        ObservableCollection<AbnormalityWidgetConfig> trays,
        GameProcessType game,
        ConfigurationAdapter configurationAdapter,
        IBodyNavigator bodyNavigator)
    {
        Trays = trays;
        Game = game;
        _configurationAdapter = configurationAdapter;
        _bodyNavigator = bodyNavigator;
    }

    public void CreateNewTray()
    {
        Trays.Add(new AbnormalityWidgetConfig());
    }

    public void DeleteTray(AbnormalityWidgetConfig tray)
    {
        Trays.Remove(tray);
    }

    public void ConfigureTray(AbnormalityWidgetConfig tray)
    {
        ConfigurationCategoryGroup configuration = _configurationAdapter.Adapt(tray).First();
        ObservableCollection<AbnormalityCategoryViewModel> abnormalities = AbnormalityCategoryViewModelBuilder.Build(Game);

        var viewModel = new AbnormalityWidgetSettingsViewModel(
            configuration: configuration.Categories.First(),
            categories: abnormalities,
            selectedAbnormalities: tray.AllowedAbnormalities
        );

        _bodyNavigator.Navigate(viewModel);
    }
}