using System;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Heimdall.Tests
{
    public class HttpActionContextBuilder
    {
        private HttpRequestMessage requestMessage;

        public HttpActionContextBuilder()
        {
            requestMessage = HttpRequestMessageBuilder
                .Instance()
                .Build();
        }

        public HttpActionContext Build()
        {
            var httpControllerContext = new HttpControllerContext { Request = requestMessage };

            var context = new HttpActionContext { ControllerContext = httpControllerContext };
            context.Request.Headers.Date = DateTime.UtcNow;

            return context;
        }

        public HttpActionContextBuilder WithRequestMessage(HttpRequestMessage requestMessage)
        {
            this.requestMessage = requestMessage;
            return this;
        }

        public static HttpActionContextBuilder Instance()
        {
            return new HttpActionContextBuilder();
        }
    }
}