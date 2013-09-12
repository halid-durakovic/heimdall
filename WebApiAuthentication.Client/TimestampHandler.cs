using System;
using System.Net.Http;

namespace WebApiAuthentication.Client
{
    public class TimestampHandler : DelegatingHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Headers.Date = new DateTimeOffset(DateTime.Now, DateTime.Now - DateTime.UtcNow);
            return base.SendAsync(request, cancellationToken);
        }
    }
}