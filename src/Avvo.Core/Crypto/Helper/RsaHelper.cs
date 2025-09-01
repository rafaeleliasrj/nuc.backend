using System.Runtime.CompilerServices;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Avvo.Core.Crypto.Extensions;
using Microsoft.Extensions.Logging;
using Avvo.Core.Crypto.Consts;

namespace Avvo.Core.Crypto.Helper
{
    public class RsaHelper
    {
        public static string Encrypt(string plainText, RSAParameters publicKey)
        {
            var rsa = new RSACryptoServiceProvider(RsaCryptoConst.SIZE);
            rsa.ImportParameters(publicKey);
            var data = Encoding.UTF8.GetBytes(plainText);
            var cypher = rsa.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public static string Decrypt(string cypherText, RSAParameters privateKey)
        {
            var dataBytes = Convert.FromBase64String(cypherText);
            var rsa = new RSACryptoServiceProvider(RsaCryptoConst.SIZE);
            rsa.ImportParameters(privateKey);
            var plainText = rsa.Decrypt(dataBytes, false);
            return Encoding.UTF8.GetString(plainText);
        }

        public static string Encrypt(string plainText, RSAParameters publicKey, ILogger logger)
        {
            try
            {
                return Encrypt(plainText, publicKey);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RsaHelper.Encrypt");
                throw;
            }
        }

        public static string Decrypt(string cypherText, RSAParameters privateKey, ILogger logger)
        {
            try
            {
                return Decrypt(cypherText, privateKey);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "RsaHelper.Decrypt");
                throw;
            }
        }
    }
}
