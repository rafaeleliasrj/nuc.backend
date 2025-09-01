using Avvo.Core.Crypto.Entities;
using Avvo.Core.Crypto.Extensions;
using Avvo.Core.Crypto.Helper;
using Avvo.Core.Crypto.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Avvo.Core.Crypto
{
    public class DecryptValues : IDecryptValues
    {
        private readonly ILogger<DecryptValues> logger;
        private readonly IGetCryptoContext getCryptoContext;
        public DecryptValues(ILogger<DecryptValues> logger, IGetCryptoContext getCryptoContext)
        {
            this.logger = logger;
            this.getCryptoContext = getCryptoContext;
        }

        public async Task<string> Execute(Guid cryptoContextId, string value, KeyType keyType = KeyType.Client)
        {
            try
            {
                var values = new Dictionary<string, object>()
                {
                    ["value"] = value,
                };

                var resultValues = await this.Execute(cryptoContextId, values, keyType);

                return resultValues["value"].ToString();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "DecryptValues.Execute");
                throw;
            }
        }

        private async Task<CryptoContext> ValidateContext(Guid cryptoContextId)
        {
            try
            {
                var cryptoContext = await this.getCryptoContext.Execute(cryptoContextId);

                if (cryptoContext == null || cryptoContext.Id == default)
                {
                    var ex = new Exception($"The Id:{cryptoContextId} not exists");

                    this.logger.LogError(ex, "DecryptValues.Execute");

                    throw ex;
                }

                return cryptoContext;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "DecryptValues.ValidateContext");
                throw;
            }
        }

        public async Task<IDictionary<string, object>> Execute(Guid cryptoContextId, IDictionary<string, object> values, KeyType keyType = KeyType.Client)
        {
            var resultValues = new Dictionary<string, object>();

            var cryptoContext = await this.ValidateContext(cryptoContextId);

            try
            {
                RSAParameters key;

                if (keyType == KeyType.Client)
                {
                    key = cryptoContext.ClientPrivateKeyString.FromPrivatekey();
                    this.logger.LogInformation($"DecryptValues.Execute Context Id: {cryptoContext.Id}, ClientPrivateKey: {cryptoContext.ClientPrivateKeyString}");
                }
                else
                {
                    key = cryptoContext.ServerPrivateKeyString.FromPrivatekey();
                    this.logger.LogInformation($"DecryptValues.Execute Context Id: {cryptoContext.Id}, ClientPrivateKey: {cryptoContext.ServerPrivateKeyString}");
                }

                foreach (var item in values)
                {
                    if (item.Value != null)
                    {
                        this.logger.LogInformation($"DecryptValues.Execute Context Id: {cryptoContext.Id}, {item.Key}: {item.Value}");
                        resultValues.Add(item.Key, RsaHelper.Decrypt(item.Value.ToString(), key, this.logger));
                    }
                }

                return resultValues;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"DecryptValues.Execute Context Id : {cryptoContextId}");
                throw;
            }
        }
    }
}
