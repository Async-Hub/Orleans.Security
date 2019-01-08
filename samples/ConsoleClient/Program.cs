using System;
using System.Net.Http;
using IdentityModel.Client;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please press 's' to start.");

            if (Console.ReadKey().Key == ConsoleKey.S)
            {
                var discoveryResponse = DiscoveryClient.GetAsync("http://localhost:5000").Result;

                if (discoveryResponse.IsError)
                {
                    Console.WriteLine(discoveryResponse.Error);
                    return;
                }

                // Request token
                var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "Client1", "MySuperSecret");

                //var tokenResponse = tokenClient.RequestClientCredentialsAsync("api1").Result;
                var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("Alice", "PassW1@rd", "api1").Result;

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }

                Console.WriteLine(tokenResponse.Json);

                // Call api
                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);

                var response = client.GetAsync("https://localhost:44322/api/values").Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(content);
                }
            }
        }
    }
}
