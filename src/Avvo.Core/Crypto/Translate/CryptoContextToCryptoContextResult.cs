using Avvo.Core.Crypto.Entities;

namespace Avvo.Core.Crypto.Translate
{
    public class CryptoContextToCryptoContextResult
    {
        public static CryptoContextResult Translate(CryptoContext context) =>
            context != null ?
            CryptoContextResult.Create(context.Id, context.ServerPrivateKeyString, context.ClientPublicKeyString, context.CreatedDate, context.ExpirationDate) :
            CryptoContextResult.Create();
    }
}
