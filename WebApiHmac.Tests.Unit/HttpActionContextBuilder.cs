using System;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace WebApiHmac.Tests.Unit
{
    public class HttpActionContextBuilder
    {
        public static HttpActionContext Build()
        {
            var httpControllerContext = new HttpControllerContext { Request = new HttpRequestMessage(HttpMethod.Post, "http://www.me.com/api") };

            var context = new HttpActionContext { ControllerContext = httpControllerContext };
            context.Request.Headers.Date = DateTime.UtcNow;

            return context;
        }
    }
}