using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Heimdall;
using Heimdall.Server;

namespace Example.IIS
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Manually install heimdall
            var authenticateRequest = new AuthenticateRequest(new DummyGetSecretFromUsername());
            GlobalConfiguration.Configuration.MessageHandlers.Add(new HmacAuthenticationHandler(authenticateRequest));
        }
    }

    public class DummyGetSecretFromUsername : IGetSecretFromUsername
    {
        public string Secret(string username)
        {
            return "secret";
        }
    }
}