using HunterPie.Core.Domain.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;

namespace HunterPie.Core.Crypto;
public class CryptoService : ICryptoService
{
    private readonly ILocalRegistry _localRegistry;

    public CryptoService(ILocalRegistry localRegistry)
    {
        _localRegistry = localRegistry;
    }

    public string Encrypt(string value)
    {
        byte[] iv = new byte[16];

        using var aes = Aes.Create();

        object? secret = _localRegistry.Get("secret");

        if (secret is not byte[] secretKey)
            throw new Exception("Failed to find secret for encrypting data");

        aes.Key = secretKey;
        aes.IV = iv;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream))
            streamWriter.Write(value);

        byte[] data = memoryStream.ToArray();

        return Convert.ToBase64String(data);
    }

    public string Decrypt(string encrypted)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(encrypted);

        using var aes = Aes.Create();

        object? secret = _localRegistry.Get("secret");

        if (secret is not byte[] secretKey)
            throw new Exception("Failed to find secret for encrypting data");

        aes.Key = secretKey;
        aes.IV = iv;
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return streamReader.ReadToEnd();
    }
}