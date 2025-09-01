using System.Security.Cryptography;
using Avvo.Core.Crypto.Entities;
using Avvo.Core.Crypto.Extensions;
using Avvo.Core.Crypto.Helper;
using Avvo.Core.Crypto.Interface;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Crypto
{
    public class EncryptValues : IEncryptValues
    {
        private readonly ILogger<EncryptValues> logger;
        private readonly IGetCryptoContext getCryptoContext;
        public EncryptValues(ILogger<EncryptValues> logger, IGetCryptoContext getCryptoContext)
        {
            this.logger = logger;
            this.getCryptoContext = getCryptoContext;
        }

        public async Task<string?> Execute(Guid cryptoContextId, string value, KeyType keyType = KeyType.Server)
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
                this.logger.LogError(ex, "EncryptValues.Execute, {ClassName}", this.GetType().Name);
                throw;
            }
        }

        public async Task<IDictionary<string, object>> Execute(Guid cryptoContextId, IDictionary<string, object> values, KeyType keyType = KeyType.Server)
        {
            try
            {
                var resultValues = new Dictionary<string, object>();

                var cryptoContext = await this.getCryptoContext.Execute(cryptoContextId);

                if (cryptoContext == null || cryptoContext.Id == Guid.Empty)
                {
                    var ex = new Exception($"The CryptoContext not exists whith the Id:{cryptoContextId}");
                    this.logger.LogError(ex, "EncryptValues.Execute");
                    throw ex;
                }

                try
                {
                    RSAParameters key;

                    if (keyType == KeyType.Server)
                    {
                        key = cryptoContext.ServerPublicKeyString.FromPublickey();
                        this.logger.LogInformation("EncryptValues.Execute Context Id: {ContextId}, ClientPrivateKey: {ClientPrivateKey}", cryptoContext.Id, cryptoContext.ServerPublicKeyString);
                    }
                    else
                    {
                        key = cryptoContext.ClientPublicKeyString.FromPublickey();
                        this.logger.LogInformation("EncryptValues.Execute Context Id: {ContextId}, ClientPrivateKey: {ClientPrivateKey}", cryptoContext.Id, cryptoContext.ClientPublicKeyString);
                    }

                    foreach (var item in values)
                    {
                        if (item.Value != null)
                        {
                            this.logger.LogInformation("EncryptValues.Execute Context Id: {ContextId}, {Key}: {Value}", cryptoContext.Id, item.Key, item.Value);
                            if (item.Value is string stringValue)
                                resultValues.Add(item.Key, RsaHelper.Encrypt(stringValue, key, this.logger));
                        }
                    }

                    return resultValues;
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "EncryptValues.Execute Context Id : {ContextId}", cryptoContextId);
                    throw;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "EncryptValues.Execute Context Id : {ContextId}", cryptoContextId);
                throw;
            }
        }
    }
}
