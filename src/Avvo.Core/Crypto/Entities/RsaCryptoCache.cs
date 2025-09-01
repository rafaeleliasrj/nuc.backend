using System;
namespace Avvo.Core.Crypto.Entities
{
    public class RsaCryptoCache
    {
        public Guid CryptoContextId { get; set; }
        public string ClientPrivateKeyString { get; set; }
        public string ClientPublicKeyString { get; set; }
        public string ServerPublicKeyString { get; set; }
        public string ServerPrivateKeyString { get; set; }
        public static RsaCryptoCache Create() => new() { };
        public static RsaCryptoCache Create(
            Guid cryptoContextId,
            string clientPrivateKeyString,
            string clientPublicKeyString,
            string serverPublicKeyString,
            string serverPrivateKeyString
        ) => new()
        {
            CryptoContextId = cryptoContextId,
            ClientPrivateKeyString = clientPrivateKeyString,
            ServerPublicKeyString = serverPublicKeyString,
            ClientPublicKeyString = clientPublicKeyString,
            ServerPrivateKeyString = serverPrivateKeyString,
        };
    }
}
