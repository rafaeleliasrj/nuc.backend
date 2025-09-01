using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avvo.Core.Crypto.Entities;

namespace Avvo.Core.Crypto.Interface
{
    public interface IDecryptValues
    {
        Task<IDictionary<string, object>> Execute(Guid cryptoContextId, IDictionary<string, object> values, KeyType keyType = KeyType.Client);
        Task<string> Execute(Guid cryptoContextId, string value, KeyType keyType = KeyType.Client);
    }
}
