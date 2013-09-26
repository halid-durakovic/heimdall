using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiAuthentication.Client.Handlers
{
    public class RequestContentMD5HeaderHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content == null)
                return await base.SendAsync(request, cancellationToken);

            var contentMD5 = await MD5Helper.ComputeHash(request.Content);

            request.Content.Headers.ContentMD5 = contentMD5;

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
