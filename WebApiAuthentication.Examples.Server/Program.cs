﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.SelfHost;
using WebApiAuthentication.Client;
using WebApiAuthentication.Client.Handlers;
using WebApiAuthentication.Server;

namespace WebApiAuthentication.Examples.Server
{
    public class DummyGetSecretForUsername : IGetSecretFromUsername
    {
        public string Secret(string username)
        {
            return "secret";
        }
    }

    class Program
    {
        static void Main()
        {
            var config = new HttpSelfHostConfiguration("http://localhost:8080");

            var authenticateRequest = new AuthenticateRequest(new DummyGetSecretForUsername());

            var qqq = GlobalConfiguration.Configuration;

            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                constraints: null,
                defaults: new { id = RouteParameter.Optional });

            config.MessageHandlers.Add(new RequestContentMD5HeaderHandler());
            config.MessageHandlers.Add(new HmacAuthenticationHandler(authenticateRequest));

            var server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();

            Succesful_Message_Signing_And_Authentication();
            Unsuccesful_Authentication();

            Console.Read();
        }

        private static void Succesful_Message_Signing_And_Authentication()
        {
            Console.WriteLine("Showing a successfully signed message authenticated server side...");

            var client = SigningHttpClientFactory.Create("username", "secret");

            var response = client.PostAsync("http://localhost:8080/api/values",
                new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("a", "b"), }))
                .Result;

            Console.WriteLine("Request: ");
            Console.WriteLine(response.RequestMessage);

            Console.WriteLine("Reeponse: ");
            Console.WriteLine(response);
            ;
            Console.WriteLine();
        }

        private static void Unsuccesful_Authentication()
        {
            Console.WriteLine("Showing a message signed with a different key and not authenticated server side...");

            var client = SigningHttpClientFactory.Create("username", "different_secret");

            var response = client.PostAsync("http://localhost:8080/api/values",
                new FormUrlEncodedContent(new[] {new KeyValuePair<string, string>("a", "b"),})).Result;

            Console.WriteLine("Request: ");
            Console.WriteLine(response.RequestMessage);

            Console.WriteLine("Reeponse: ");
            Console.WriteLine(response);

            Console.WriteLine();
        }
    }
}
