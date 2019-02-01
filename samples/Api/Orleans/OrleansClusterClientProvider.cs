using System;
using System.Threading.Tasks;
using GrainsInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using Orleans.Security;
using Orleans.Security.ClusterClient;

namespace Api.Orleans
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class OrleansClusterClientProvider
    {
        private static IClusterClient _client;

        private static readonly int _initializeAttemptsBeforeFailing = 5;

        private static IClusterClient Build(IHttpContextAccessor contextAccessor, 
            OAuth2EndpointInfo oAuth2EndpointInfo)
        {
            var builder = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "Orleans.Security.Test";
                    options.ServiceId = "ServiceId1";
                })
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IUserGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureServices(services =>
                {
                    services.AddOrleansClusterAuthorization(AuthorizationConfig.ConfigureOptions);

                    services.AddSingleton<Func<IHttpContextAccessor>>(serviceProvider => () => contextAccessor);
                    services.AddSingleton(oAuth2EndpointInfo);
                    services.AddScoped<IAccessTokenProvider, AspNetCoreAccessTokenProvider>();
                });

            return builder.Build();
        }

        private static IClusterClient TryToConnect(IHttpContextAccessor httpContextAccessor, 
            OAuth2EndpointInfo oAuth2EndpointInfo)
        {
            var attempt = 0;
            //var logger = loggerFactory.CreateLogger<OrleansClusterClientProvider>();

            while (true)
            {
                try
                {
                    var client = Build(httpContextAccessor, oAuth2EndpointInfo);
                    client.Connect().Wait();

                    //logger.LogInformation("Client successfully connect to silo host");

                    return client;
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException is SiloUnavailableException)
                    {
                        attempt++;
                        //logger.LogError(ex, ex.Message);

                        if (attempt > _initializeAttemptsBeforeFailing)
                        {
                            throw;
                        }

                        Task.Delay(TimeSpan.FromSeconds(1));
                    }

                    //logger.LogError(ex, ex.Message);
                }
            }
        }

        public static void StartClientWithRetries(out IClusterClient client, 
            IHttpContextAccessor httpContextAccessor, OAuth2EndpointInfo oAuth2EndpointInfo)
        {
            if (_client != null && _client.IsInitialized)
            {
                client = _client;
            }
            _client = TryToConnect(httpContextAccessor, oAuth2EndpointInfo);
            client = _client;
        }
    }
}