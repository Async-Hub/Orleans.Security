using System;
using System.Net;
using System.Threading.Tasks;
using Grains;
using GrainsInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Security;
using Orleans.Security.Clustering;

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
            var identityServer4Info = new IdentityServer4Info("https://localhost:5001",
                "Api2", @"C%#4>#2x-kH(d9HaQqs?3Wt@NLT.\x$[");

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
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(UserGrain).Assembly).WithReferences())
                .ConfigureServices(services =>
                {
                    
                    services.AddOrleansClusteringAuthorization(identityServer4Info,
                        config =>
                        {
                            config.ConfigureAuthorizationOptions = AuthorizationConfig.ConfigureOptions;
                            config.TracingEnabled = true;
                        });
                })
                // Configure logging with any logging framework that supports Microsoft.Extensions.Logging.
                // In this particular case it logs using the Microsoft.Extensions.Logging.Console package.
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
