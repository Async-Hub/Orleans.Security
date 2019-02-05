using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Orleans.Security.TokenVerification.IntegrationTests.Configuration
{
    public class TestIdentityServer4Builder
    {
        public static TestServer BuildNew()
        {
            return BuildTestServer();
        }

        private static TestServer BuildTestServer()
        {
            var builder = new WebHostBuilder()
                .UseUrls("http://localhost:5001")
                .ConfigureServices(services =>
                {
                    services.AddIdentityServer()
                        .AddDeveloperSigningCredential()
                        .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                        .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                        .AddInMemoryClients(IdentityServerConfig.GetClients())
                        .AddTestUsers(IdentityServerConfig.GetUsers());

                    services.AddMvcCore();
                })
                .Configure(app =>
                {
                    app.UseIdentityServer();
                    app.UseMvcWithDefaultRoute();
                });

            var identityServer4 = new TestServer(builder);
            identityServer4.CreateHandler();
            identityServer4.BaseAddress = new Uri("http://localhost:5001");

            return identityServer4;
        }
    }
}
