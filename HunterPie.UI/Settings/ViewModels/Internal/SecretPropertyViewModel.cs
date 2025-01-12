using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class SecretPropertyViewModel : ConfigurationPropertyViewModel
{
    public Secret Secret { get; }

    public SecretPropertyViewModel(Secret secret)
    {
        Secret = secret;
    }
}