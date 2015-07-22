using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Heimdall.Server
{
    public interface IAuthenticateRequest
    {
        Task<bool> IsAuthenticated(HttpRequestMessage request);
    }

    public class AuthenticateRequest : IAuthenticateRequest
    {
        private readonly IBuildRequestSignature buildRequestSignature;
        private readonly IGetSecretFromUsername getSecretFromUsername;
        private readonly ICalculateHashes hashCalculator;

        public AuthenticateRequest(IGetSecretFromUsername getSecretFromUsername)
            : this(new HashCalculator(), new BuildRequestSignature(), getSecretFromUsername)
        {
        }

        public AuthenticateRequest(ICalculateHashes hashCalculator, IBuildRequestSignature buildRequestSignature,
            IGetSecretFromUsername getSecretFromUsername)
        {
            this.hashCalculator = hashCalculator;
            this.buildRequestSignature = buildRequestSignature;
            this.getSecretFromUsername = getSecretFromUsername;
        }

        public async Task<bool> IsAuthenticated(HttpRequestMessage request)
        {
            if ((request.Headers.Authorization == null) || (!request.Headers.Contains(HeaderNames.UsernameHeader)))
                return await Task.FromResult(false);

            var secret = getSecretFromUsername.Secret(request.Headers.GetValues(HeaderNames.UsernameHeader).FirstOrDefault());

            if (secret == null)
                return await Task.FromResult(false);

            if ((request.Content != null) && (request.Content.Headers.ContentMD5 != null))
            {
                var isValidHash = await hashCalculator.IsValidHash(request);
                if (!isValidHash)
                    return await Task.FromResult(false);
            }

            var signature = buildRequestSignature.Build(secret, request);
            if (signature != request.Headers.Authorization.Parameter)
                return await Task.FromResult(false);

            return await Task.FromResult(true);
        }
    }
}