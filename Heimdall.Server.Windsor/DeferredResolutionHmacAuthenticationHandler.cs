using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentlyWindsor;

namespace Heimdall.Server.Windsor
{
    public class DeferredResolutionHmacAuthenticationHandler : HmacAuthenticationHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (AuthenticateRequest == null)
            {
                if (!FluentWindsor.ServiceLocator.Kernel.HasComponent(typeof (IGetSecretFromUsername)))
                    throw new CannotResolveSecretFromUsernamesException("Have you registered an implementation for IGetSecretFromUsername? Could not find one ... ");

                var getSecretFromUsername = FluentWindsor.ServiceLocator.Resolve<IGetSecretFromUsername>();
                AuthenticateRequest = new AuthenticateRequest(getSecretFromUsername);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}