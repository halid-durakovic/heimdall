using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Heimdall.Client.Handlers
{
    public class RequestContentMd5HeaderHandler : DelegatingHandler
    {
        private readonly HashCalculator hashCalculator = new HashCalculator();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content == null)
                return base.SendAsync(request, cancellationToken);
            request.Content.Headers.ContentMD5 = hashCalculator.ComputeHash(request.Content);
            return base.SendAsync(request, cancellationToken);
        }
    }
}