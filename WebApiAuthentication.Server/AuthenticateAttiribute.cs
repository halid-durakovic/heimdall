using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApiAuthentication.Server
{
    public class AuthenticateAttiribute : ActionFilterAttribute
    {
        public IBuildRequestSignature BuildRequestSignature { get; set; }
        public IGetSecretFromUsername GetSecretFromUsername { get; set; }

        public AuthenticateAttiribute()
        {
            BuildRequestSignature = new BuildRequestSignature();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!IsAuthenticated(actionContext))
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            base.OnActionExecuting(actionContext);
        }

        private bool IsAuthenticated(HttpActionContext actionContext)
        {
            if ((actionContext.Request.Headers.Authorization == null) || (!actionContext.Request.Headers.Contains(HeaderNames.UsernameHeader)))
                return false;

            var secret = GetSecretFromUsername.Secret(actionContext.Request.Headers.GetValues(HeaderNames.UsernameHeader).FirstOrDefault());

            if ((secret == null) || (actionContext.Request.Headers.Authorization.Parameter != BuildRequestSignature.Build(secret, actionContext.Request)))
                return false;

            return true;
        }
    }
}
