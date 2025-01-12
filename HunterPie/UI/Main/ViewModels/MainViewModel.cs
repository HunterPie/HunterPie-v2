using HunterPie.Core.Client;
using HunterPie.Features.Account.UseCase;
using HunterPie.UI.Architecture;
using HunterPie.UI.Header.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.UI.Main.ViewModels;

internal class MainViewModel : ViewModel
{
    private readonly IRemoteAccountConfigUseCase _remoteConfigService;

    public HeaderViewModel HeaderViewModel { get; }

    private ViewModel? _contentViewModel;
    public ViewModel? ContentViewModel { get => _contentViewModel; set => SetValue(ref _contentViewModel, value); }

    public MainViewModel(
        HeaderViewModel headerViewModel,
        IRemoteAccountConfigUseCase remoteConfigService)
    {
        HeaderViewModel = headerViewModel;
        _remoteConfigService = remoteConfigService;
    }

    public async Task GracefulShutdown()
    {
        ConfigManager.SaveAll();
        await _remoteConfigService.UploadAsync();
    }
}