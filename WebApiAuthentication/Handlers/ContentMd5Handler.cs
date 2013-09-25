using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WebApiAuthentication.Handlers
{
    public class ContentMd5Handler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request.Content == null)
                return await base.SendAsync(request, cancellationToken);

            var content = await request.Content.ReadAsByteArrayAsync();
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(content);
            request.Content.Headers.ContentMD5 = hash;
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
