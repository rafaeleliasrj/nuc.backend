using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Avvo.Core.Host.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string RequestBodyToString(this HttpRequest request)
        {
            var returnValue = string.Empty;
            request.EnableBuffering();
            //ensure we read from the begining of the stream - in case a reader failed to read to end before us.
            request.Body.Position = 0;
            //use the leaveOpen parameter as true so further reading and processing of the request body can be done down the pipeline
            using (var stream = new StreamReader(request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
            {
                returnValue = stream.ReadToEnd();
            }
            //reset position to ensure other readers have a clear view of the stream
            request.Body.Position = 0;
            return returnValue;
        }
    }
}
