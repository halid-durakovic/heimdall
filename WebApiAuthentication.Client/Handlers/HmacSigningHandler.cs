using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApiAuthentication.Client.Handlers
{
    public class HmacSigningHandler : DelegatingHandler
    {
        private readonly string secret;
        private readonly IBuildRequestSignature buildRequestSignature;

        public HmacSigningHandler(string secret)
            : this(secret, new BuildRequestSignature())
        { }

        public HmacSigningHandler(string secret, IBuildRequestSignature buildRequestSignature)
        {
            this.secret = secret;
            this.buildRequestSignature = buildRequestSignature;
        }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var signature = buildRequestSignature.Build(secret, request);

            request.Headers.Authorization = new AuthenticationHeaderValue(HeaderNames.AuthenticationScheme, signature);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
