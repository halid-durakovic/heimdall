using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApiAuthentication.Server
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
            if (!authenticateRequest.IsAuthenticated(request))
            {
                var response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "x");
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme));
                return response;
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
