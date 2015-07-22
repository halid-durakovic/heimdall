using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentlyWindsor;

namespace Heimdall.Server.Windsor
{
    public class DeferredResolutionHmacAuthenticationHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (!FluentWindsor.ServiceLocator.Kernel.HasComponent(typeof (IGetSecretFromUsername)))
                throw new CannotResolveSecretFromUsernamesException("Have you registered an implementation for IGetSecretFromUsername? Could not find one ... ");

            var getSecretFromUsername = FluentWindsor.ServiceLocator.Resolve<IGetSecretFromUsername>();
            var authenticateRequest = new AuthenticateRequest(getSecretFromUsername);
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