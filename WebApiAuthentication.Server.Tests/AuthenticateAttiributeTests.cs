using System.Net;
using NUnit.Framework;
using WebApiAuthentication.Tests;

namespace WebApiAuthentication.Server.Tests
{
    [TestFixture]
    public class AuthenticateAttiributeTests
    {
        [Test]
        public void returns_not_authorised_for_requests_without_authentication_header()
        {
            var request = HttpRequestMessageBuilder.Instance().Build();
            request.Headers.Authorization = null;

            var actionContext = HttpActionContextBuilder
                .Instance()
                .WithRequestMessage(request)
                .Build();

            var attribute = new AuthenticateAttiribute();

            attribute.OnActionExecuting(actionContext);

            Assert.That(actionContext.Response, Is.Not.Null);
            Assert.That(actionContext.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
