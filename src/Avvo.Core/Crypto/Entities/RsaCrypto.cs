using Avvo.Core.Crypto.Consts;
using Avvo.Core.Crypto.Extensions;
using System.Security.Cryptography;

namespace Avvo.Core.Crypto.Entities
{
    public class RsaCrypto
    {
        private RSAParameters clientPrivateKey;
        private RSAParameters clientPublicKey;
        private RSAParameters serverPrivateKey;
        private RSAParameters serverPublicKey;
        private string clientPrivateKeyString;
        private string clientPublicKeyString;
        private string serverPrivateKeyString;
        private string serverPublicKeyString;

        public string GetClientPublickey()
        {
            if (string.IsNullOrEmpty(this.clientPublicKeyString))
            {
                this.clientPublicKeyString = this.clientPublicKey.ToPublicKey();
            }

            return this.clientPublicKeyString;
        }

        public string GetClientPrivatekey()
        {
            if (string.IsNullOrEmpty(this.clientPrivateKeyString))
            {
                this.clientPrivateKeyString = this.clientPrivateKey.ToPrivateKey();
            }

            return this.clientPrivateKeyString;
        }

        public string GetServerPublickey()
        {
            if (string.IsNullOrEmpty(this.serverPublicKeyString))
            {
                this.serverPublicKeyString = this.serverPublicKey.ToPublicKey();
            }

            return this.serverPublicKeyString;
        }

        public string GetServerPrivatekey()
        {
            if (string.IsNullOrEmpty(this.serverPrivateKeyString))
            {
                this.serverPrivateKeyString = this.serverPrivateKey.ToPrivateKey();
            }

            return this.serverPrivateKeyString;
        }

        public RsaCrypto()
        {
            using (RSACryptoServiceProvider rsaClient = new(RsaCryptoConst.SIZE))
            {
                clientPrivateKey = rsaClient.ExportParameters(true);
                clientPublicKey = rsaClient.ExportParameters(false);
            }

            using (RSACryptoServiceProvider rsaServer = new(RsaCryptoConst.SIZE))
            {
                serverPrivateKey = rsaServer.ExportParameters(true);
                serverPublicKey = rsaServer.ExportParameters(false);
            }
        }

        public static RsaCrypto Create() => new();
    }
}
