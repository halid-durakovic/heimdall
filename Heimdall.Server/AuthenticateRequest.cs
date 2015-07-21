using System.Linq;
using System.Net.Http;

namespace Heimdall.Server
{
    public interface IAuthenticateRequest
    {
        bool IsAuthenticated(HttpRequestMessage request);
    }

    public class AuthenticateRequest : IAuthenticateRequest
    {
        private readonly ICalculateHashes hashCalculator;
        private readonly IBuildRequestSignature buildRequestSignature;
        private readonly IGetSecretFromUsername getSecretFromUsername;

        public AuthenticateRequest(IGetSecretFromUsername getSecretFromUsername)
            : this(new HashCalculator(), new BuildRequestSignature(), getSecretFromUsername)
        { }

        public AuthenticateRequest(ICalculateHashes hashCalculator, IBuildRequestSignature buildRequestSignature, IGetSecretFromUsername getSecretFromUsername)
        {
            this.hashCalculator = hashCalculator;
            this.buildRequestSignature = buildRequestSignature;
            this.getSecretFromUsername = getSecretFromUsername;
        }

        public bool IsAuthenticated(HttpRequestMessage request)
        {
            if ((request.Headers.Authorization == null) || (!request.Headers.Contains(HeaderNames.UsernameHeader)))
                return false;

            var secret = getSecretFromUsername.Secret(request.Headers.GetValues(HeaderNames.UsernameHeader).FirstOrDefault());

            if (secret == null)
                return false;

            if ((request.Content != null) && (request.Content.Headers.ContentMD5 != null))
                if (!hashCalculator.IsValidHash(request))
                    return false;

            var signature = buildRequestSignature.Build(secret, request);

            if (signature != request.Headers.Authorization.Parameter)
                return false;

            return true;
        }
    }
}
