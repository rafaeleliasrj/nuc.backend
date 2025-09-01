using System;

namespace Avvo.Core.Crypto.Entities
{
    public class CryptoContext
    {
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public RsaCrypto RsaCrypto { get; internal set; }
        public CryptoContext(bool createKeys = true)
        {
            if (createKeys)
            {
                this.RsaCrypto = RsaCrypto.Create();
            }
        }

        public string ClientPrivateKeyString { get; set; }
        public string ClientPublicKeyString { get; set; }
        public string ServerPrivateKeyString { get; set; }
        public string ServerPublicKeyString { get; set; }

        public static CryptoContext Create(
        ) => new(false)
        { };

        public static CryptoContext Create(
            DateTime createdDate,
            DateTime expirationDate
        ) => new(true)
        {
            Id = Guid.NewGuid(),
            CreatedDate = createdDate,
            ExpirationDate = expirationDate,
        };
        public static CryptoContext Create(
            Guid id,
            DateTime? createdDate,
            DateTime? expirationDate,
            RsaCrypto rsaCrypto
        ) => new(false)
        {
            Id = id,
            CreatedDate = createdDate,
            ExpirationDate = expirationDate,
            RsaCrypto = rsaCrypto,
        };

        public static CryptoContext Create(
            Guid id,
            string clientPrivateKeyString,
            string clientPublicKeyString,
            string serverPrivateKeyString,
            string serverPublicKeyString
        ) => new(false)
        {
            Id = id,
            ClientPrivateKeyString = clientPrivateKeyString,
            ClientPublicKeyString = clientPublicKeyString,
            ServerPrivateKeyString = serverPrivateKeyString,
            ServerPublicKeyString = serverPublicKeyString,
        };
    }
}
