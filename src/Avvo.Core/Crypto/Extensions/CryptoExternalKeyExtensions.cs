using System;
using Avvo.Core.Crypto.Helper;

namespace Avvo.Core.Crypto.Extensions
{
    public static class CryptoExternalKeyExtensions
    {
        public static T DecryptByExternalKey<T>(this object value, string privateKey)
        {
            var result = RsaHelper.Decrypt(value.ToString(), privateKey.FromPrivatekey());

            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static string EncryptByExternalKey(this object value, string publicKey)
        {
            var result = RsaHelper.Encrypt(value.ToString(), publicKey.FromPublickey());

            return result;
        }
    }
}
