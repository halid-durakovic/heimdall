using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Heimdall.Server
{
    public class HmacAuthenticationHandler : DelegatingHandler
    {
        protected IAuthenticateRequest AuthenticateRequest;

        protected HmacAuthenticationHandler()
        {
        }

        public HmacAuthenticationHandler(IAuthenticateRequest authenticateRequest)
        {
            if (authenticateRequest == null) throw new ArgumentNullException(nameof(authenticateRequest));
            this.AuthenticateRequest = authenticateRequest;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (HeimdallConfig.IgnorePath(request))
                return await base.SendAsync(request, cancellationToken);

            var isAuthenticated = await AuthenticateRequest.IsAuthenticated(request);
            if (!isAuthenticated)
            {
                var response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized API call");
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme));
                return await Task.FromResult(response);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
