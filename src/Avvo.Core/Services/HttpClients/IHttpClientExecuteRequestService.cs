using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Avvo.Core.Services.HttpClients
{
    public interface IHttpClientExecuteRequestService
    {
        Task<HttpRequestResult<T>> Execute<T>(
            HttpMethod method,
            string url,
            Dictionary<String, String> queryParams = null,
            Dictionary<String, String> headers = null,
            object body = null,
            bool serializeBody = true,
            RequestType requestType = RequestType.Json
        ) where T : class;
    }
}
