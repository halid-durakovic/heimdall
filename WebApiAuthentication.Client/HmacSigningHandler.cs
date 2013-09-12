using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApiAuthentication.Client
{
    public class HmacSigningHandler : DelegatingHandler
    {
        private readonly IGetSecretFromKey getSecretFromKey;
        private readonly IBuildMessageRepresentation buildMessageRepresentation;
        private readonly ICalculateSignature calculateSignature;

        public HmacSigningHandler(string signingKey, IGetSecretFromKey getSecretFromKey)
            : this(signingKey, getSecretFromKey, new BuildMessageRepresentation(), new CalculateSignature())
        { }

        public HmacSigningHandler(string signingKey, IGetSecretFromKey getSecretFromKey, IBuildMessageRepresentation buildMessageRepresentation, ICalculateSignature calculateSignature)
        {
            this.getSecretFromKey = getSecretFromKey;
            this.buildMessageRepresentation = buildMessageRepresentation;
            this.calculateSignature = calculateSignature;
            SigningKey = signingKey;
        }

        public string SigningKey { get; private set; }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var secret = getSecretFromKey.Secret(SigningKey);

            var representation = buildMessageRepresentation.Build(request);
            var signature = calculateSignature.Generate(secret, representation);

            //todo: refactor into separate handler - more testable
            if(!request.Headers.Contains(HeaderNames.UsernameHeader))
                request.Headers.Add(HeaderNames.UsernameHeader, SigningKey);

            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, signature);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
