using System;
using System.Threading.Tasks;
using Avvo.Core.Crypto.Entities;

namespace Avvo.Core.Crypto.Interface
{
    public interface ICreateCryptoContext
    {
        Task<CryptoContextResult> Execute(DateTime createdDate, DateTime expirationDate);
    }
}
