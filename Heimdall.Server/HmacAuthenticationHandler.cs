using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Heimdall.Server.Interfaces;

namespace Heimdall.Server
{
    public class HmacAuthenticationHandler : DelegatingHandler
    {
        private readonly IAuthenticateRequest authenticateRequest;

        public HmacAuthenticationHandler(IAuthenticateRequest authenticateRequest)
        {
            this.authenticateRequest = authenticateRequest;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (!authenticateRequest.IsAuthenticated(request))
            {
                var response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized API call");
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme));
                return Task.Factory.StartNew(() => response);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}