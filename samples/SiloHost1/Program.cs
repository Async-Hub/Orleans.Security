using System;
using System.Net;
using System.Threading.Tasks;
using Grains;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Security.Cluster;

namespace SiloHost1
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Console.Title = "SiloHost1";

                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var builder = new SiloHostBuilder()
                // Use localhost clustering for a single local silo
                .UseLocalhostClustering()
                // Configure ClusterId and ServiceId
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Orleans.Security.Test";
                    options.ServiceId = "ServiceId1";
                })
                // Configure connectivity
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(UserGrain).Assembly).WithReferences())
                .AddIncomingGrainCallFilter<IncomingGrainCallAuthorizationFilter>()
                // Configure logging with any logging framework that supports Microsoft.Extensions.Logging.
                // In this particular case it logs using the Microsoft.Extensions.Logging.Console package.
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
