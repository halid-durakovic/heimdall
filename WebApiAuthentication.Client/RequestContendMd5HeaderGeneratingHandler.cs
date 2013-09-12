using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApiAuthentication.Client
{
    public class RequestContendMd5HeaderGeneratingHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               System.Threading.CancellationToken cancellationToken)
        {
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
