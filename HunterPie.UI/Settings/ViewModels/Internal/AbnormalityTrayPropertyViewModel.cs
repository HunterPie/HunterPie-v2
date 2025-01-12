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
    public ObservableCollection<AbnormalityWidgetConfig> Trays { get; }
    public GameProcessType Game { get; }

    public AbnormalityTrayPropertyViewModel(
        ObservableCollection<AbnormalityWidgetConfig> trays,
        GameProcessType game
    )
    {
        Trays = trays;
        Game = game;
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
        ConfigurationCategory configuration = ConfigurationAdapter.Adapt(tray).First();
        ObservableCollection<AbnormalityCategoryViewModel> abnormalities = AbnormalityCategoryViewModelBuilder.Build(Game);

        var viewModel = new AbnormalityWidgetSettingsViewModel(
            configuration: configuration,
            categories: abnormalities,
            selectedAbnormalities: tray.AllowedAbnormalities
        );

        Navigator.Body.Navigate(viewModel);
    }
}