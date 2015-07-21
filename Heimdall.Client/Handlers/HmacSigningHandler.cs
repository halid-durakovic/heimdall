using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Heimdall.Client.Handlers
{
    public class HmacSigningHandler : DelegatingHandler
    {
        private readonly IBuildRequestSignature buildRequestSignature;
        private readonly string secret;

        public HmacSigningHandler(string secret)
            : this(secret, new BuildRequestSignature())
        {
        }

        public HmacSigningHandler(string secret, IBuildRequestSignature buildRequestSignature)
        {
            this.secret = secret;
            this.buildRequestSignature = buildRequestSignature;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string signature = buildRequestSignature.Build(secret, request);

            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, signature);

            return base.SendAsync(request, cancellationToken);
        }
    }
}