using HunterPie.Core.Crypto;
using HunterPie.Core.Logger;
using HunterPie.Core.Utils;
using HunterPie.Core.Vault;
using System.Runtime.InteropServices;
using System.Text;
using static HunterPie.Platforms.Windows.Api.Adv.AdvApi32;
using Credential = HunterPie.Core.Vault.Model.Credential;
using NativeCredential = HunterPie.Platforms.Windows.Api.Adv.AdvApi32.Credential;

namespace HunterPie.Platforms.Windows.Vault;

internal class WindowsCredentialVault : ICredentialVault
{
    private const string OWNER_NAME = "HunterPie";
    private readonly ICryptoService _cryptoService;

    public WindowsCredentialVault(ICryptoService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    public void Create(string username, string password)
    {
        string encryptedPassword = _cryptoService.Encrypt(password);
        byte[] passwordBytes = Encoding.Unicode.GetBytes(encryptedPassword);

        var credential = new NativeCredential
        {
            AttributeCount = 0,
            Attributes = IntPtr.Zero,
            Comment = IntPtr.Zero,
            TargetAlias = IntPtr.Zero,
            Type = CredentialType.Generic,
            Persist = (uint)CredentialPersistence.LocalMachine,
            CredentialBlobSize = (uint)passwordBytes.Length,
            TargetName = Marshal.StringToCoTaskMemUni(OWNER_NAME),
            CredentialBlob = Marshal.StringToCoTaskMemUni(encryptedPassword),
            Username = Marshal.StringToCoTaskMemUni(username)
        };

        bool success = CredWriteW(ref credential, 0);

        Marshal.FreeCoTaskMem(credential.TargetName);
        Marshal.FreeCoTaskMem(credential.CredentialBlob);
        Marshal.FreeCoTaskMem(credential.Username);

        if (!success)
            Log.Error("Failed to save credentials due to error: {0}", Marshal.GetLastWin32Error());

    }

    public void Delete() => CredDeleteW(OWNER_NAME, CredentialType.Generic, 0);

    public Credential? Get()
    {
        bool success = CredReadW(OWNER_NAME, CredentialType.Generic, 0, out IntPtr handle);

        if (!success)
            return null;

        if (handle == IntPtr.Zero)
            return null;

        NativeCredential credential = MarshalHelper.BufferToStructures<NativeCredential>(handle, 1)
                                                   .First();

        if (credential.CredentialBlob == IntPtr.Zero)
            return null;

        int passwordLength = (int)credential.CredentialBlobSize / 2;
        string? username = Marshal.PtrToStringUni(credential.Username);

        if (username is not { })
            return null;

        string? password = Marshal.PtrToStringUni(credential.CredentialBlob, passwordLength);

        if (password is not { })
            return null;

        string decryptedPassword = _cryptoService.Decrypt(password);

        CredFree(handle);

        return new Credential(
            Username: username,
            Password: decryptedPassword
        );
    }
}