using System;

namespace Avvo.Core.Crypto.Entities
{
    public class CryptoContextResult
    {
        public Guid Id { get; set; }
        public string? ServerPrivateKey { get; set; }
        public string? ClientPublicKey { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public static CryptoContextResult Create() => new();
        public static CryptoContextResult Create(
            Guid id,
            string serverPrivateKey,
            string clientPublicKey,
            DateTime? createdDate,
            DateTime? expirationDate
        ) => new()
        {
            Id = id,
            ServerPrivateKey = serverPrivateKey,
            ClientPublicKey = clientPublicKey,
            CreatedDate = createdDate,
            ExpirationDate = expirationDate,
        };
    }
}
