using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Heimdall.Server
{
    public class HmacAuthenticationHandler : DelegatingHandler
    {
        private readonly IAuthenticateRequest authenticateRequest;

        public HmacAuthenticationHandler(IAuthenticateRequest authenticateRequest)
        {
            this.authenticateRequest = authenticateRequest;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var isAuthenticated = await authenticateRequest.IsAuthenticated(request);
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
