using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace Heimdall
{
    public interface IBuildMessageRepresentation
    {
        string Build(HttpRequestMessage request);
    }

    /// <summary>
    /// HTTP METHOD\n +
    /// HTTP PATH\n +
    /// Content-Type\n +  
    /// Content-MD5\n +  
    /// Timestamp\n +
    /// </summary>
    public class BuildMessageRepresentation : IBuildMessageRepresentation
    {
        public string Build(HttpRequestMessage request)
        {
            var md5 = request.Content == null || request.Content.Headers.ContentMD5 == null
                ? "" : Convert.ToBase64String(request.Content.Headers.ContentMD5);

            var contentType = (request.Content == null || request.Content.Headers.ContentType == null)
                ? "" : request.Content.Headers.ContentType.MediaType;

            string date;

            if (request.Headers.Contains(HeaderNames.CustomDateHeader))
                date = request.Headers.GetValues(HeaderNames.CustomDateHeader).FirstOrDefault();
            else
                date = request.Headers.Date == null
                   ? "" : request.Headers.Date.Value.UtcDateTime.ToString(CultureInfo.InvariantCulture);

            var path = request.RequestUri.AbsolutePath.Replace("//", "/");

            return string.Join("\n",
                request.Method,
                path,
                contentType,
                md5,
                date
                );
        }
    }
}
