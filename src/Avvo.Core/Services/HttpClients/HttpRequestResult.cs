using System;
using System.Net.Http.Headers;

namespace Avvo.Core.Services.HttpClients
{
    public class HttpRequestResult<T> where T : class
    {
        public int StatusCode { get; set; }

        public String StatusDescription { get; set; }

        public T Data { get; set; }

        public string RawData { get; set; }

        public bool IsSuccessStatusCode { get; set; }
        public HttpResponseHeaders ResponseHeaders { get; internal set; }
    }
}
