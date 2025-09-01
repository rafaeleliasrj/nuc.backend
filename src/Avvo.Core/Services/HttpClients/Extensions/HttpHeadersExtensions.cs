using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Avvo.Core.Services.HttpClients.Extensions
{
    public static class HttpHeadersExtensions
    {
        public static void PrepareHeader(this HttpRequestHeaders headers, Dictionary<String, String> Headers = null)
        {
            if (Headers != null)
                foreach (var h in Headers)
                    headers.Add(h.Key, h.Value);

        }
    }
}
