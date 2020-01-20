using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ConsoleClient
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Please press 's' to start.");

            if (Console.ReadKey().Key == ConsoleKey.S)
            {
                var discoveryClient = new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:5001")
                };

                var discoveryResponse = await discoveryClient.GetDiscoveryDocumentAsync();

                if (discoveryResponse.IsError)
                {
                    Console.WriteLine(discoveryResponse.Error);
                    return;
                }


                var client = new HttpClient();

                var passwordTokenRequest = new PasswordTokenRequest()
                {
                    ClientId = "ConsoleClient",
                    ClientSecret = "KHG+TZ8htVx2h3^!vJ65",
                    Address = discoveryResponse.TokenEndpoint,
                    UserName = "Alice",
                    Password = "Pass123$",
                    Scope = "Api1 Api1.Read Api1.Write Orleans"
                };

                var tokenResponse = await client.RequestPasswordTokenAsync(passwordTokenRequest);

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                    return;
                }

                Console.WriteLine(tokenResponse.Json);

                // Call API
                client.SetBearerToken(tokenResponse.AccessToken);

                var response = await client.GetAsync("https://localhost:5002/api/user/1");
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
