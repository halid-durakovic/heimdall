using System;
using System.Net.Http;

namespace Heimdall.Client.Handlers
{
    public class TimestampHandler : DelegatingHandler
    {
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            request.Headers.Date = new DateTimeOffset(now, TimeZoneInfo.Local.GetUtcOffset(now));
            return base.SendAsync(request, cancellationToken);
        }
    }
}