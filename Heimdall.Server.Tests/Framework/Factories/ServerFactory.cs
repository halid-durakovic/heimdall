using System.Web.Http;
using System.Web.Http.SelfHost;
using Heimdall.Server.Tests.Framework.Providers;

namespace Heimdall.Server.Tests.Framework.Factories
{
    public class ServerFactory
    {
        public static HttpSelfHostServer CreateServer(string url)
        {
            var config = new HttpSelfHostConfiguration(url);

            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                constraints: null,
                defaults: new { id = RouteParameter.Optional });

            var authenticateRequest = new AuthenticateRequest(new GetSecretFromUsername());
            config.MessageHandlers.Add(new HmacAuthenticationHandler(authenticateRequest));

            HeimdallConfig.EnableByPassWebApiCorsAndImplementOptions(yesOrNo:true);

            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();

            return server;
        }
    }
}