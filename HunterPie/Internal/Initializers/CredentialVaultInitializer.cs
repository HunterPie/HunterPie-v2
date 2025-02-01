using HunterPie.Core.Vault;
using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

// TODO: Migrate everything to use ICredentialVault and then delete this
internal class CredentialVaultInitializer : IInitializer
{
    private readonly ICredentialVault _credentialVault;

    public CredentialVaultInitializer(ICredentialVault credentialVault)
    {
        _credentialVault = credentialVault;
    }

    public Task Init()
    {
        _ = new CredentialVaultService(_credentialVault);

        return Task.CompletedTask;
    }
}