using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Heimdall;
using Heimdall.Client;
using Heimdall.Server;

namespace Example.Server
{
    public class DummyGetSecretForUsername : IGetSecretFromUsername
    {
        public string Secret(string username)
        {
            return "secret";
        }
    }

    internal class Program
    {
        private static HttpClient CreateClient()
        {
            var client = HeimdallClientFactory.Create("username", "secret");
            return client;
        }

        // If this fails try running the following command prompt with elevated privs
        // netsh http add urlacl url=http://+:8080/ user=DOMAIN\user
        private static HttpSelfHostServer CreateServer(string url)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            var authenticateRequest = new AuthenticateRequest(new DummyGetSecretForUsername());

            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                constraints: null,
                defaults: new { id = RouteParameter.Optional });

            config.MessageHandlers.Add(new HmacAuthenticationHandler(authenticateRequest));

            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
            return server;
        }

        private static void Main()
        {
            var client = CreateClient();
            CreateServer("http://localhost:8080");

            const int numberOfRequests = 100;
            var stopwatch = Stopwatch.StartNew();

            for (var requestCounter = 0; requestCounter < numberOfRequests; requestCounter++)
            {
                int counter = requestCounter;
                SignAndSendMessage(client, "http://localhost:8080/api/values",
                                   () =>
                                   {
                                       if (counter == (numberOfRequests - 1))
                                       {
                                           stopwatch.Stop();
                                           Task.Factory.StartNew(() =>
                                           {
                                               Thread.Sleep(250);
                                               Console.WriteLine(string.Format("Executed in {0} millisecond(s)", stopwatch.Elapsed.TotalMilliseconds));
                                           });
                                       }
                                   });
            }

            Unsuccesful_Authentication();

            Console.Read();
        }

        private static void SignAndSendMessage(HttpClient client, string url, Action onComplete)
        {
            Console.WriteLine("***** Showing a successfully signed message authenticated server side... *****");

            client.PostAsync(url, new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("a", "b"), }))
                  .ContinueWith((task) =>
                  {
                      Console.WriteLine("Request: ");
                      Console.WriteLine(task.Result.RequestMessage);
                      Console.WriteLine("Response: ");
                      Console.WriteLine(task.Result);
                      Console.WriteLine();
                      onComplete();
                  });
        }

        private static void Unsuccesful_Authentication()
        {
            Console.WriteLine("***** Showing a message signed with a different key and not authenticated server side... *****");

            var client = HeimdallClientFactory.Create("username", "different_secret");

            var response = client.PostAsync("http://localhost:8080/api/values",
                                            new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("a", "b"), })).Result;

            Console.WriteLine("Request: ");
            Console.WriteLine(response.RequestMessage);
            Console.WriteLine("Response: ");
            Console.WriteLine(response);
            Console.WriteLine();
        }
    }
    public class ValuesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]string value)
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
