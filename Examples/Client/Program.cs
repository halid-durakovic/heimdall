using System;
using System.Collections.Generic;
using System.Net.Http;
using WebApiAuthentication.Client;

namespace WebApiAuthentication.Examples.Client
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Send signed request to requestbin...");
            Console.WriteLine("Request:");
            Console.WriteLine(SendSignedPostRequest().RequestMessage);
            Console.Read();
        }

        private static HttpResponseMessage SendSignedPostRequest()
        {
            HttpClient client = SigningHttpClientFactory.Create("myusername", "mysecret");

            var content = new FormUrlEncodedContent(
                new[]
                    {
                        new KeyValuePair<string, string>("firstName", "Alex"),
                        new KeyValuePair<string, string>("lastName", "Brown")
                    });

            return client.PostAsync("http://requestb.in/14nmm871", content)
                         .Result;
        }
    }
}