using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Avvo.Core.Services.HttpClients.Extensions
{
    public static class QueryParamExtensions
    {
        public static string PrepareQueryParams(this string url, Dictionary<String, String> queryParams = null)
        {
            if (queryParams == null)
                return url;

            StringBuilder queryString = new StringBuilder();
            foreach (var p in queryParams)
            {
                if (queryString.Length > 0)
                    queryString.Append("&");

                queryString.Append(p.Key);
                queryString.Append("=");
                queryString.Append(HttpUtility.UrlEncode(p.Value));
            }

            queryString.Insert(0, url.Contains("?") ? "&" : "?");
            queryString.Insert(0, url);
            return queryString.ToString();
        }

    }
}
