using System.Net.Http;

namespace WebApiAuthentication.Client
{
    public class UsernameHandler : TimestampHandler
    {
        private readonly string username;

        public UsernameHandler(string username)
        {
            this.username = username;
        }

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(HeaderNames.UsernameHeader))
                request.Headers.Add(HeaderNames.UsernameHeader, username);

            return base.SendAsync(request, cancellationToken);
        }
    }
}