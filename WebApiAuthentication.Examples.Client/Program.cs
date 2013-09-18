using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiAuthentication.Client;

namespace WebApiAuthentication.Examples.Client
{
    class Program
    {
        static void Main()
        {
            var client = SigningHttpClientFactory.Create("myusername", "mysecret");

            var content = new FormUrlEncodedContent(new[]
                                          {
                                              new KeyValuePair<string, string>("firstName", "Alex"),
                                              new KeyValuePair<string, string>("lastName", "Brown") 
                                          });

            var result = client.PostAsync("http://requestb.in/14nmm871", content)
                .Result;
        }
    }
}
