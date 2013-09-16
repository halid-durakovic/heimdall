using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApiAuthentication.Client.Handlers
{
    public class HmacSigningHandler : DelegatingHandler
    {
        private readonly IGetSecretFromKey getSecretFromKey;
        private readonly IBuildMessageRepresentation buildMessageRepresentation;
        private readonly ICalculateSignature calculateSignature;
        private readonly string signingKey; //username, for example

        public HmacSigningHandler(string signingKey, IGetSecretFromKey getSecretFromKey)
            : this(signingKey, getSecretFromKey, new BuildMessageRepresentation(), new CalculateSignature())
        { }

        public HmacSigningHandler(string signingKey, IGetSecretFromKey getSecretFromKey, IBuildMessageRepresentation buildMessageRepresentation, ICalculateSignature calculateSignature)
        {
            this.getSecretFromKey = getSecretFromKey;
            this.buildMessageRepresentation = buildMessageRepresentation;
            this.calculateSignature = calculateSignature;
            this.signingKey = signingKey;
        }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var secret = getSecretFromKey.Secret(signingKey);

            var representation = buildMessageRepresentation.Build(request);
            
            var signature = calculateSignature.Generate(secret, representation);

            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, signature);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
