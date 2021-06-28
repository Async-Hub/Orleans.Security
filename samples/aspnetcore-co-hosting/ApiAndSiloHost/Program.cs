using System;
using System.Net;
using ApiAndSiloHost.Orleans;
using Grains;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Security;
using Orleans.Security.Clustering;

namespace ApiAndSiloHost
{
    public static class Program
    {
        private static IdentityServer4Info _identityServer4Info;

        public static void Main(string[] args)
        {
            _identityServer4Info = new IdentityServer4Info("http://localhost:5000",
                "Api1", @"TFGB=?Gf3UvH+Uqfu_5p", "Orleans");

            // TODO: Audience validation logic
            // https://leastprivilege.com/2020/06/15/the-jwt-profile-for-oauth-2-0-access-tokens-and-identityserver/
            //_identityServer4Info = new IdentityServer4Info("https://localhost:5001",
            //    "Orleans", "@3x3g*RLez$TNU!_7!QW", "Orleans");

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
                            services.AddOrleansCoHostedClusterAuthorization(_identityServer4Info,
                                config =>
                                {
                                    config.ConfigureAuthorizationOptions = AuthorizationConfig.ConfigureOptions;
                                    config.ConfigureAccessTokenVerifierOptions = options =>
                                    {
                                        options.InMemoryCacheEnabled = true;
                                        options.AllowOfflineValidation = false;
                                    };

                                    config.TracingEnabled = true;
                                    config.ConfigureSecurityOptions = options =>
                                    {
                                        options.RequireHttps = false;
                                    };
                                });
                            
                            services.AddHttpContextAccessor();
                            services.AddSingleton<Func<IHttpContextAccessor>>(serviceProvider => 
                                serviceProvider.GetService<IHttpContextAccessor>);
                            services.AddTransient<IAccessTokenProvider, AspNetCoreAccessTokenProvider>();
                        })
                        // Configure connectivity
                        .Configure<EndpointOptions>(
                            options => { options.AdvertisedIPAddress = IPAddress.Loopback; });
                })
                .ConfigureLogging(logging => logging.AddConsole());
    }
}
