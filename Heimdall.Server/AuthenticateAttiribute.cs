using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApiAuthentication.Server
{
    public class AuthenticateAttiribute : ActionFilterAttribute
    {
        //for property injection by DI container
        public IAuthenticateRequest AuthenticateRequest { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (AuthenticateRequest == null)
                throw new NullReferenceException("AuthenticateRequest not set. This can be caused by an incorrectly configured DI container");

            if (!AuthenticateRequest.IsAuthenticated(actionContext.Request))
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            base.OnActionExecuting(actionContext);
        }

    }
}
