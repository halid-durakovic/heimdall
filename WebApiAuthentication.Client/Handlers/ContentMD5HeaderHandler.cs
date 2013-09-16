using System.Net.Http;
using System.Threading.Tasks;

namespace WebApiAuthentication.Client.Handlers
{
    public class ContentMD5HeaderHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               System.Threading.CancellationToken cancellationToken)
        {
            if (request.Content == null)
                return await base.SendAsync(request, cancellationToken);

            var contentBytes = request.Content
                .ReadAsByteArrayAsync()
                .Result;

            var contentMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider()
                .ComputeHash(contentBytes);

            request.Content.Headers.ContentMD5 = contentMD5;

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
