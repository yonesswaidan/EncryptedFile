using System;
using System.IO;
using System.Security.Cryptography;

namespace EncryptedFileApp.Services
{
    public class FileEncryptionService
    {
        public (byte[] EncryptedData, byte[] Key, byte[] IV) Encrypt(byte[] data)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256; // Bruger 256-bit nøgle 
            aes.GenerateKey(); // Generer en ny, og tilfældig krypteringsnøgle
            aes.GenerateIV();  // Generer en ny, og tilfældig IV

            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            return (ms.ToArray(), aes.Key, aes.IV);
        }

        public byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256; // Matcher nøglestørrelse ved dekryptering
            aes.Key = key;     // Brug den korrekte nøgle og skal hentes sikkert
            aes.IV = iv;       // Brug den korrekte IV og skal hentes sikkert

            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(encryptedData, 0, encryptedData.Length);
            cryptoStream.FlushFinalBlock();

            return ms.ToArray();
        }
    }
}
