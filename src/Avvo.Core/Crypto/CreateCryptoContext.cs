using Avvo.Core.Crypto.Entities;
using Avvo.Core.Crypto.Interface;
using Avvo.Core.Crypto.Translate;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Crypto
{
    public class CreateCryptoContext : ICreateCryptoContext
    {
        private const string KeyPrefix = "crypto_";
        private readonly ILogger<CreateCryptoContext> logger;
        private readonly IMemoryCache memoryCache;

        public CreateCryptoContext(ILogger<CreateCryptoContext> logger, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
        }

        public async Task<CryptoContextResult> Execute(DateTime createdDate, DateTime expirationDate)
        {
            try
            {
                var context = CryptoContext.Create(createdDate, expirationDate);
                var cacheExpires = expirationDate.Subtract(createdDate);

                if (context.RsaCrypto == null)
                {
                    var ex = new Exception("RsaCrypto in context is null");
                    this.logger.LogError(ex, "CreateCryptoContext.Execute");
                    throw ex;
                }

                this.logger.LogInformation(
                    "Generated CryptoContext Id:{0}, Created Date: {1}, Expiration Date:{2}, Cache Expires:{3}",
                    context.Id, createdDate, expirationDate, cacheExpires);

                var rsaCryptoCache = RsaCryptoCache.Create(
                    context.Id,
                    context.RsaCrypto.GetClientPrivatekey(),
                    context.RsaCrypto.GetClientPublickey(),
                    context.RsaCrypto.GetServerPublickey(),
                    context.RsaCrypto.GetServerPrivatekey());

                SetCache(KeyPrefix + context.Id.ToString(), rsaCryptoCache, cacheExpires);

                return CryptoContextToCryptoContextResult.Translate(
                    CryptoContext.Create(
                        rsaCryptoCache.CryptoContextId,
                        rsaCryptoCache.ClientPrivateKeyString,
                        rsaCryptoCache.ClientPublicKeyString,
                        rsaCryptoCache.ServerPrivateKeyString,
                        rsaCryptoCache.ServerPublicKeyString));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "CreateCryptoContext.Execute");
                throw;
            }
        }

        private void SetCache(string key, object data, TimeSpan expires)
        {
            try
            {
                memoryCache.Set(key, data, expires);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "CreateCryptoContext.SetCache Error.");
                throw;
            }
        }
    }
}
