using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace WebApiAuthentication.Server
{
    public class AuthenticateAttiribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            actionContext.Response = response;
        }
    }
}
