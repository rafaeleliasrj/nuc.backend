using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Avvo.Core.Crypto.Helper
{
    public class AesHelper
    {
        public static string EncryptPassword(string password, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = keyBytes.Take(16).ToArray();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] cipherBytes = encryptor.TransformFinalBlock(passwordBytes, 0, passwordBytes.Length);
                    return Convert.ToBase64String(cipherBytes);
                }
            }
        }

        public static string DecryptPassword(string encryptedPassword, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] cipherBytes = Convert.FromBase64String(encryptedPassword);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = keyBytes.Take(16).ToArray();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] passwordBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    return Encoding.UTF8.GetString(passwordBytes);
                }
            }
        }
    }
}
