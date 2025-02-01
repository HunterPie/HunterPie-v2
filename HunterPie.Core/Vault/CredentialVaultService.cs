using HunterPie.Core.Vault.Model;
using System;

namespace HunterPie.Core.Vault;

[Obsolete("Use ICredentialVault instead")]
internal class CredentialVaultService
{
    private static ICredentialVault _vault;

    public CredentialVaultService(ICredentialVault vault)
    {
        _vault = vault;
    }

    public static void SaveCredential(string username, string password) => _vault.Create(username, password);
    public static void DeleteCredential() => _vault.Delete();
    public static Credential? GetCredential() => _vault.Get();

}