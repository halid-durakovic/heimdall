using System.Linq;
using System.Net.Http;

namespace WebApiAuthentication.Server
{
    public interface IAuthenticateRequest
    {
        bool IsAuthenticated(HttpRequestMessage request);
    }

    public class AuthenticateRequest : IAuthenticateRequest
    {
        private readonly IBuildRequestSignature buildRequestSignature;
        private readonly IGetSecretFromUsername getSecretFromUsername;

        public AuthenticateRequest(IGetSecretFromUsername getSecretFromUsername)
            : this(new BuildRequestSignature(), getSecretFromUsername)
        { }

        public AuthenticateRequest(IBuildRequestSignature buildRequestSignature, IGetSecretFromUsername getSecretFromUsername)
        {
            this.buildRequestSignature = buildRequestSignature;
            this.getSecretFromUsername = getSecretFromUsername;
        }

        public bool IsAuthenticated(HttpRequestMessage request)
        {

            if ((request.Headers.Authorization == null) || (!request.Headers.Contains(HeaderNames.UsernameHeader)))
                return false;

            var secret = getSecretFromUsername.Secret(request.Headers.GetValues(HeaderNames.UsernameHeader).FirstOrDefault());

            if ((secret == null) || (request.Headers.Authorization.Parameter != buildRequestSignature.Build(secret, request)))
                return false;

            return true;
        }
    }
}
