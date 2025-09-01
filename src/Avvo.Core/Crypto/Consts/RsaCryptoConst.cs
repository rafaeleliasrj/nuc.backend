using System;

namespace Avvo.Core.Crypto.Consts
{
    public class RsaCryptoConst
    {
        public static readonly int SIZE = Environment.GetEnvironmentVariable("RSA_CRYPTO_SIZE") == null ? 512 : short.Parse(Environment.GetEnvironmentVariable("RSA_CRYPTO_SIZE"));
    }
}
