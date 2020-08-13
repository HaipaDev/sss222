using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace PatchKit.UnityEditorExtension.Core
{
public static class Encryption
{
    [NotNull]
    public static byte[] EncryptString(
        [NotNull] string text,
        [NotNull] string key)
    {
        if (text == null)
        {
            throw new ArgumentNullException("text");
        }

        if (key == null)
        {
            throw new ArgumentNullException("key");
        }

        byte[] keyBytes;

        using (SHA256 sha256 = SHA256.Create())
        {
            Assert.IsNotNull(sha256);
            keyBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(key));
        }

        byte[] textBytes = Encoding.UTF8.GetBytes(text);

        return Encrypt(textBytes, keyBytes);
    }

    [NotNull]
    public static string DecryptString(
        [NotNull] byte[] data,
        [NotNull] string key)
    {
        if (data == null)
        {
            throw new ArgumentNullException("data");
        }

        if (key == null)
        {
            throw new ArgumentNullException("key");
        }

        byte[] keyBytes;

        using (SHA256 sha256 = SHA256.Create())
        {
            Assert.IsNotNull(sha256);

            keyBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(key));
        }

        return Encoding.UTF8.GetString(Decrypt(data, keyBytes));
    }

    [NotNull]
    public static byte[] Encrypt(byte[] data, byte[] key)
    {
        if (data == null)
        {
            throw new ArgumentNullException("data");
        }

        if (key == null)
        {
            throw new ArgumentNullException("key");
        }

        using (Aes aes = Aes.Create())
        {
            Assert.IsNotNull(aes);

            aes.Key = key;

            ICryptoTransform cryptoTransform =
                aes.CreateEncryptor(aes.Key, aes.IV);

            return aes.IV.ToList()
                .Concat(PerformCryptography(cryptoTransform, data).ToList())
                .ToArray();
        }
    }

    [NotNull]
    public static byte[] Decrypt([NotNull] byte[] data, [NotNull] byte[] key)
    {
        if (data == null)
        {
            throw new ArgumentNullException("data");
        }

        if (key == null)
        {
            throw new ArgumentNullException("key");
        }

        byte[] iv = data.ToList().Take(16).ToArray();
        byte[] encryptedData = data.ToList().Skip(16).ToArray();

        using (Aes aes = Aes.Create())
        {
            Assert.IsNotNull(aes);

            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform cryptoTransform =
                aes.CreateDecryptor(aes.Key, aes.IV);

            return PerformCryptography(cryptoTransform, encryptedData);
        }
    }

    [NotNull]
    private static byte[] PerformCryptography(
        [NotNull] ICryptoTransform cryptoTransform,
        [NotNull] byte[] data)
    {
        if (cryptoTransform == null)
        {
            throw new ArgumentNullException("cryptoTransform");
        }

        if (data == null)
        {
            throw new ArgumentNullException("data");
        }

        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(
                memoryStream,
                cryptoTransform,
                CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }
}
}