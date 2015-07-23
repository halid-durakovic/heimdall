using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentlyWindsor;
using FluentlyWindsor.Mvc;
using FluentlyWindsor.WebApi;
using Heimdall;

namespace Example.IIS
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // See ./Providers to see how GetSecretFromUsername is installed
            FluentWindsor
                .NewContainer(Assembly.GetExecutingAssembly())
                .WithArrayResolver()
                .WithInstallers()
                .RegisterApiControllers(GlobalConfiguration.Configuration)
                .RegisterMvcControllers(ControllerBuilder.Current, "Example.IIS.Controllers")
                .Create();
        }
    }
}