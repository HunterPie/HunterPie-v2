using HunterPie.Core.Client;
using HunterPie.Features.Account.UseCase;
using HunterPie.UI.Settings;
using HunterPie.UI.Settings.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Config;

internal class LocalAccountConfig
{
    private readonly AccountConfig _accountConfig;
    private readonly IAccountUseCase _accountUseCase;
    private readonly ConfigurationAdapter _configurationAdapter;

    private const string ACCOUNT_CONFIG = @"internal\account_config.json";

    public LocalAccountConfig(
        AccountConfig accountConfig,
        IAccountUseCase accountUseCase,
        ConfigurationAdapter configurationAdapter)
    {
        _accountConfig = accountConfig;
        _accountUseCase = accountUseCase;
        _configurationAdapter = configurationAdapter;
        RegisterConfiguration();
    }

    private void RegisterConfiguration()
    {
        ConfigManager.Register(ACCOUNT_CONFIG, _accountConfig);
        ConfigManager.BindConfiguration(ACCOUNT_CONFIG, _accountConfig);
    }

    public async Task<ObservableCollection<ConfigurationCategoryGroup>> BuildAccountConfigAsync()
    {
        bool isLoggedIn = await _accountUseCase.IsValidSessionAsync();

        return isLoggedIn switch
        {
            true => _configurationAdapter.Adapt(_accountConfig),
            _ => new ObservableCollection<ConfigurationCategoryGroup>()
        };
    }
}