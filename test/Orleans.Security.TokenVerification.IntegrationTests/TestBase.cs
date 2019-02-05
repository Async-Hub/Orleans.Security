using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.TestHost;
using Orleans.Security.TokenVerification.IntegrationTests.Configuration;

namespace Orleans.Security.TokenVerification.IntegrationTests
{
    public class TestBase
    {
        private readonly TestServer _identityServer4;

        private readonly HttpClient _identityServer4Client;

        public TestBase()
        {
            _identityServer4 = TestIdentityServer4Builder.BuildNew();
            _identityServer4Client = _identityServer4.CreateClient();
        }

        protected async Task<string> RequestJwtTokenAsync(string clientId, string clientSecret,
            string scope)
        {
            var discoveryResponse = await _identityServer4Client.GetDiscoveryDocumentAsync();

            var response = await _identityServer4Client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest()
                {
                    Address = discoveryResponse.TokenEndpoint,
                    Scope = scope,
                    ClientId = clientId,
                    ClientSecret = clientSecret
                });

            return response.AccessToken;
        }
    }
}
