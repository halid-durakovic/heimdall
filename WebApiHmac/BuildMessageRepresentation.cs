using System;
using System.Globalization;
using System.Net.Http;

namespace WebApiHmac
{
    /// <summary>
    /// HTTP PATH
    /// HTTP METHOD\n +
    /// Content-MD5\n +  
    /// Timestamp\n +
    /// </summary>
    public class BuildMessageRepresentation
    {
        public string Build(HttpRequestMessage request)
        {
            var md5 = (request.Content == null || request.Content.Headers.ContentMD5 == null)
            ? ""
            : Convert.ToBase64String(request.Content.Headers.ContentMD5);

            var date = request.Headers.Date.Value.UtcDateTime;

            return string.Join("\n",
                request.RequestUri.AbsolutePath.ToLower(),
                request.Method,
                md5,
                date.ToString(CultureInfo.InvariantCulture)
                );
        }
    }
}
