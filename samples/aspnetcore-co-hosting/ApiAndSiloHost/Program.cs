using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiAndSiloHost.Orleans;
using Grains;
using GrainsInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Security;
using Orleans.Security.CoHosting;

namespace ApiAndSiloHost
{
    public static class Program
    {
        private static IdentityServer4Info _identityServer4Info;

        public static void Main(string[] args)
        {
            _identityServer4Info = new IdentityServer4Info("https://localhost:5001",
                "Orleans", "@3x3g*RLez$TNU!_7!QW", "Orleans");

            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseOrleans(siloBuilder =>
                {
                    // Configure Orleans
                    siloBuilder
                        .UseLocalhostClustering()
                        // Configure ClusterId and ServiceId
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "Orleans.Security.Test";
                            options.ServiceId = "ServiceId1";
                        })
                        .ConfigureApplicationParts(parts =>
                            parts.AddApplicationPart(typeof(UserGrain).Assembly).WithReferences())
                        .ConfigureServices(services =>
                        {
                            services.AddOrleansClusteringAuthorization(_identityServer4Info,
                                config =>
                                {
                                    config.ConfigureAuthorizationOptions = AuthorizationConfig.ConfigureOptions;
                                    config.ConfigureAccessTokenVerifierOptions = options =>
                                    {
                                        options.InMemoryCacheEnabled = true;
                                    };

                                    config.TracingEnabled = true;
                                });
                            services.AddSingleton<Func<IHttpContextAccessor>>(serviceProvider => 
                                serviceProvider.GetService<IHttpContextAccessor>);

                            services.AddSingleton<IAccessTokenProvider, AspNetCoreAccessTokenProvider>();
                        })
                        // Configure connectivity
                        .Configure<EndpointOptions>(
                            options => { options.AdvertisedIPAddress = IPAddress.Loopback; });
                })
                .ConfigureLogging(logging => logging.AddConsole());
    }
}
