using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Security.Client;

namespace Orleans.Security.IntegrationTests.Extensions
{
    internal static class TestClientBuilder
    {
        internal static IClusterClient Client { get; private set; }

        internal static async Task StartClient()
        {
            var client = new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = TestClusterOptions.ClusterId;
                    options.ServiceId = TestClusterOptions.ServiceId;
                }).ConfigureServices(services =>
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

                    services.AddSingleton<IAccessTokenProvider, FakeAccessTokenProvider>();
                    services.AddSingleton(new OAuth2EndpointInfo("authorityUrl",
                        "clientScopeName", "clientSecret"));
                })
                .UseLocalhostClustering()
                .Build();

            await client.Connect();

            Client = client;
        }
    }
}
