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
            aes.KeySize = 256;
            aes.GenerateKey();
            aes.GenerateIV();

            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            return (ms.ToArray(), aes.Key, aes.IV);
        }

        public byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.Key = key;
            aes.IV = iv;

            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(encryptedData, 0, encryptedData.Length);
            cryptoStream.FlushFinalBlock();

            return ms.ToArray();
        }
    }
}