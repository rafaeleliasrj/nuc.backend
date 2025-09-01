using Avvo.Core.Crypto.Entities;
using Avvo.Core.Crypto.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;

namespace Avvo.Core.Crypto
{
    public class GetCryptoContext : IGetCryptoContext
    {
        private const string KeyPrefix = "crypto_";
        private readonly ILogger<GetCryptoContext> logger;
        private readonly IMemoryCache memoryCache;

        public GetCryptoContext(ILogger<GetCryptoContext> logger, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
        }

        public async Task<CryptoContext> Execute(Guid id)
        {
            var rsaCryptoCache = await GetCacheAsync(id.ToString());

            if (rsaCryptoCache == null ||
                string.IsNullOrEmpty(rsaCryptoCache.ClientPrivateKeyString) ||
                string.IsNullOrEmpty(rsaCryptoCache.ClientPublicKeyString) ||
                string.IsNullOrEmpty(rsaCryptoCache.ServerPublicKeyString) ||
                string.IsNullOrEmpty(rsaCryptoCache.ServerPrivateKeyString))
            {
                var ex = new Exception($"The RSA Key does not exist in Crypto Context with Id: {id}");
                logger.LogError(ex, "GetCryptoContext.Execute Error retrieving crypto context.");
                throw ex;
            }

            try
            {
                return CryptoContext.Create(
                    id,
                    rsaCryptoCache.ClientPrivateKeyString,
                    rsaCryptoCache.ClientPublicKeyString,
                    rsaCryptoCache.ServerPrivateKeyString,
                    rsaCryptoCache.ServerPublicKeyString
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetCryptoContext.Execute Error creating CryptoContext.");
                throw;
            }
        }

        private Task<RsaCryptoCache> GetCacheAsync(string key)
        {
            try
            {
                RsaCryptoCache rsaCrypto = null;

                var retry = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                return retry.ExecuteAsync(() =>
                {
                    if (!memoryCache.TryGetValue(KeyPrefix + key, out rsaCrypto))
                    {
                        var ex = new Exception($"The RSA Key is null for Id: {key}");
                        logger.LogError(ex, "GetCryptoContext.GetCacheAsync");
                        throw ex;
                    }

                    return Task.FromResult(rsaCrypto);
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetCryptoContext.GetCacheAsync Error accessing cache.");
                throw;
            }
        }
    }
}
