using System;
using System.Collections.Generic;
using System.Net.Http;
using FluentlyWindsor;
using Heimdall.Client;

namespace Example.Client
{
    internal class Program
    {
        private static void Main()
        {
            FluentWindsor.NewContainer(typeof(Program).Assembly).WithArrayResolver().WithInstallers().Create();
            Console.WriteLine("Send signed request to requestbin...");
            Console.WriteLine("Request:");
            Console.WriteLine(SendSignedPostRequest().RequestMessage);
            Console.Read();
        }

        private static HttpResponseMessage SendSignedPostRequest()
        {
            var client = FluentWindsor.ServiceLocator.Resolve<IHeimdallClientFactory>().Create("username", "secret");
            var content = new FormUrlEncodedContent(
                new[]
                    {
                        new KeyValuePair<string, string>("firstName", "Alex"),
                        new KeyValuePair<string, string>("lastName", "Brown")
                    });

            return client.PostAsync("http://requestb.in/14nmm871", content).Result;
        }
    }
}
