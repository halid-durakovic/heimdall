using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApiAuthentication.Client.Handlers
{
    public class HmacSigningHandler : DelegatingHandler
    {
        private readonly IBuildMessageRepresentation buildMessageRepresentation;
        private readonly ICalculateSignature calculateSignature;
        private readonly string secret;

        public HmacSigningHandler(string secret)
            : this(secret, new BuildMessageRepresentation(), new CalculateSignature())
        { }

        public HmacSigningHandler(string secret, IBuildMessageRepresentation buildMessageRepresentation, ICalculateSignature calculateSignature)
        {
            this.buildMessageRepresentation = buildMessageRepresentation;
            this.calculateSignature = calculateSignature;

            this.secret = secret;
        }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var representation = buildMessageRepresentation.Build(request);

            var signature = calculateSignature.Generate(secret, representation);

            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, signature);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
