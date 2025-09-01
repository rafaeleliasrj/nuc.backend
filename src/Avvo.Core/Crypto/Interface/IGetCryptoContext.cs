using System;
using System.Threading.Tasks;
using Avvo.Core.Crypto.Entities;

namespace Avvo.Core.Crypto.Interface
{
    public interface IGetCryptoContext
    {
        Task<CryptoContext> Execute(Guid id);
    }
}
