using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avvo.Core.Commons.Utils;
using Avvo.Core.Services.Extensions;
using Avvo.Core.Services.HttpClients.Extensions;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Services.HttpClients
{
    public class HttpClientExecuteRequestService : IHttpClientExecuteRequestService

    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;

        #region "Consts"
        private const string APPLICATION_JSON = "application/json";
        private const string APPLICATION_XML = "application/xml";
        private const string APPLICATION_TEXT = "text/plain";
        private const string APPLICATION_SOAP = "application/soap+xml";
        private const string TEXT_XML = "text/xml";
        private const string APPLICATION_X_WWW_FORM_URLENCODED = "application/x-www-form-urlencoded";
        #endregion

        public HttpClientExecuteRequestService(HttpClient httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        #region "Private methods"
        private string GetMediaType(RequestType requestType)
        {
            var mediaType = APPLICATION_JSON;

            switch (requestType)
            {
                case RequestType.Xml:
                    mediaType = APPLICATION_XML;
                    break;
                case RequestType.Text:
                    mediaType = APPLICATION_TEXT;
                    break;
                case RequestType.Soap:
                    mediaType = APPLICATION_SOAP;
                    break;
                case RequestType.TextXml:
                    mediaType = TEXT_XML;
                    break;
                case RequestType.FormUrlEncoded:
                    mediaType = APPLICATION_X_WWW_FORM_URLENCODED;
                    break;
            }
            return mediaType;
        }
        private Dictionary<string, string> CompletHeaders(Dictionary<string, string> inbountHeaders)
        {
            var headers = new Dictionary<string, string>();

            var landscape = EnvironmentVariables.Get("LANDSCAPE");
            var environment = EnvironmentVariables.Get("ENVIRONMENT");
            var applicationName = EnvironmentVariables.Get("APPLICATION_NAME");
            var applicationVersion = EnvironmentVariables.Get("VERSION");

            if (!string.IsNullOrEmpty(landscape))
            {
                headers.Add("x-requester-landscape", landscape);
            }

            if (!string.IsNullOrEmpty(environment))
            {
                headers.Add("x-requester-environment", environment);
            }

            if (!string.IsNullOrEmpty(applicationName))
            {
                headers.Add("x-requester-application-name", applicationName);
            }

            if (!string.IsNullOrEmpty(applicationVersion))
            {
                headers.Add("x-requester-application-version", applicationVersion);
            }

            if (inbountHeaders != null)
            {
                foreach (var item in headers)
                    inbountHeaders.Add(item.Key, item.Value);

                return inbountHeaders;
            }
            else
                return headers;
        }

        #endregion

        #region "Public methods"
        public async Task<HttpRequestResult<T>> Execute<T>(
            HttpMethod method,
            string url,
            Dictionary<String, String> queryParams = null,
            Dictionary<String, String> headers = null,
            object body = null,
            bool serializeBody = true,
            RequestType requestType = RequestType.Json

        ) where T : class
        {
            var mediaType = this.GetMediaType(requestType);

            url = queryParams != null ? url.PrepareQueryParams(queryParams) : url;

            var httpRequest = new HttpRequestMessage(method, url);

            headers = this.CompletHeaders(headers);

            httpRequest.Headers.PrepareHeader(headers);

            if (body != null)
            {
                if (requestType == RequestType.FormUrlEncoded)
                    httpRequest.Content = new FormUrlEncodedContent((Dictionary<string, string>)body);
                else
                {
                    var payload = serializeBody ? body.Serialize() : (string)body;

                    httpRequest.Content = new StringContent(
                        payload,
                        Encoding.UTF8,
                        mediaType
                    );
                }
            }

            try
            {
                var ret = new HttpRequestResult<T>();
                var response = await this.httpClient.SendAsync(httpRequest);
                ret.StatusCode = (int)response.StatusCode;
                ret.StatusDescription = response.ReasonPhrase;
                ret.IsSuccessStatusCode = response.IsSuccessStatusCode;
                ret.RawData = await response.Content.ReadAsStringAsync();
                ret.ResponseHeaders = response.Headers;

                if (response.IsSuccessStatusCode)
                    try
                    {
                        ret.Data = await response.Content.ReadAsJsonAsync<T>();
                    }
                    catch
                    {
                        ret.Data = null;
                    }
                else
                {
                    this.logger.LogInformation(
                        "{0}.ExecuteRequest Failed {1}-{2} Status {3}-{4} Response Body {5}",
                        this.GetType().Name,
                        httpRequest.Method,
                        httpRequest.RequestUri,
                        response.StatusCode,
                        response.ReasonPhrase,
                        ret.RawData
                    );

                }
                return ret;
            }
            catch (Exception ex)
            {
                this.logger.LogError(
                    ex,
                    "{0}.ExecuteRequest Unexpected error while making request {1}-{2}",
                    this.GetType().Name,
                    httpRequest.Method,
                    httpRequest.RequestUri);
                throw;
            }
        }
        #endregion
    }
}
