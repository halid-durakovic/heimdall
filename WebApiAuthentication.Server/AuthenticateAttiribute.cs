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
            if (!isAuthenticated(actionContext))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionContext.Response = response;
            }

            base.OnActionExecuting(actionContext);
        }

        private bool isAuthenticated(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
                return false;

            if (!actionContext.Request.Headers.Contains(HeaderNames.UsernameHeader))
                return false;

            var usernameHeader = actionContext.Request.Headers.GetValues(HeaderNames.UsernameHeader)
                .FirstOrDefault();

            var secret = GetSecretFromUsername.Secret(usernameHeader);

            if (secret == null)
                return false;

            var headerSignature = actionContext.Request.Headers.Authorization.Parameter;

            var signature = BuildRequestSignature.Build(secret, actionContext.Request);

            if (signature != headerSignature)
                return false;

            return true;
        }
    }
}
