using System.Net;
using System.Threading.Tasks;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Security.Clustering;

namespace Orleans.Security.IntegrationTests.Extensions
{
    internal static class TestClusterBuilder
    {
        internal static async Task<ISiloHost> StartSilo()
        {
            // Define the cluster configuration
            var builder = new SiloHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddOrleansClusteringAuthorization(
                        config =>
                        {
                            config.ConfigureAuthorizationOptions = AuthorizationTestConfig.ConfigureOptions;
                            config.ConfigureAccessTokenVerifierOptions = options =>
                            {
                                options.InMemoryCacheEnabled = true;
                            };
                        },
                        AuthorizationTestConfig.ConfigureServices);

                })
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {

                    options.ClusterId = TestClusterOptions.ClusterId;
                    options.ServiceId = TestClusterOptions.ServiceId;

                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback);

            var host = builder.Build();

            await host.StartAsync();

            return host;
        }
    }
}
