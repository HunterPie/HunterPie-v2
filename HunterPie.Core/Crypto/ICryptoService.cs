namespace HunterPie.Core.Crypto;

public interface ICryptoService
{
    /// <summary>
    /// Encrypts data using the user's HunterPie installation key.
    /// </summary>
    /// <param name="value">Data to be encrypted</param>
    /// <returns>Encrypted data as a Base64 encoded string</returns>
    public string Encrypt(string value);

    /// <summary>
    /// Decrypts a Base64 encoded encrypted string using the user's installation key.
    /// </summary>
    /// <param name="encrypted">Data to be decrypted</param>
    /// <returns>Decrypted data</returns>
    public string Decrypt(string encrypted);
}