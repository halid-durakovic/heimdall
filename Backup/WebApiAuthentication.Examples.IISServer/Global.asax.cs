using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using WebApiAuthentication.Server;

namespace WebApiAuthentication.Examples.IISServer
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

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