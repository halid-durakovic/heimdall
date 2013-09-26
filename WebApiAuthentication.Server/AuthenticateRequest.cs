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

            if (secret == null)
                return false;

            //check MD5
            //only if the content AND the header MD5 is not null
            if ((request.Content != null) && (request.Content.Headers.ContentMD5 != null))
                if (!MD5Helper.IsMd5Valid(request).Result)
                    return false;

            var signature = buildRequestSignature.Build(secret, request);

            if (signature != request.Headers.Authorization.Parameter)
                return false;

            return true;
        }
    }
}
