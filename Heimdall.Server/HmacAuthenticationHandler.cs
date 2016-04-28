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
            if (authenticateRequest == null) throw new ArgumentNullException("authenticateRequest");
            AuthenticateRequest = authenticateRequest;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (HeimdallConfig.ByPassWebApiCorsAndImplementOptions && request.Method.ToString().ToUpper() == "OPTIONS")
            {
                var response = request.CreateResponse(HttpStatusCode.OK, string.Empty);
                AddHeadersToResponse(response);
                return response;
            }

            if (HeimdallConfig.IgnorePath(request))
            {
                var response = await base.SendAsync(request, cancellationToken);
                AddHeadersToResponse(response);
                return response;
            }

            if (HeimdallConfig.IgnoreVerb(request))
            {
                var response = await base.SendAsync(request, cancellationToken);
                AddHeadersToResponse(response);
                return response;
            }

            if (HeimdallConfig.IgnoreVerbAndPath(request))
            {
                var response = await base.SendAsync(request, cancellationToken);
                AddHeadersToResponse(response);
                return response;
            }

            var isAuthenticated = await AuthenticateRequest.IsAuthenticated(request);
            if (!isAuthenticated)
            {
                var response = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized API call");
                AddHeadersToResponse(response);
                return response;
            }

            var authorisedResponse = await base.SendAsync(request, cancellationToken);
            AddHeadersToResponse(authorisedResponse);
            return authorisedResponse;
        }

        private static void AddHeadersToResponse(HttpResponseMessage response)
        {
            if (!response.Headers.Contains(HeaderNames.AuthenticationScheme))
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme));

            if (HeimdallConfig.ByPassWebApiCorsAndImplementOptions)
            {
                if (!response.Headers.Contains(HeaderNames.AccessControlAllowOrigin))
                    response.Headers.Add(HeaderNames.AccessControlAllowOrigin, "*");

                if (!response.Headers.Contains(HeaderNames.AccessControlAllowMethods))
                    response.Headers.Add(HeaderNames.AccessControlAllowMethods, "GET,POST,PUT,DELETE,OPTIONS");

                if (!response.Headers.Contains(HeaderNames.AccessControlAllowHeaders))
                    response.Headers.Add(HeaderNames.AccessControlAllowHeaders, "Authorization,Content-Type,X-ApiAuth-Date,X-ApiAuth-Username,Content-MD5");
            }
        }
    }
}