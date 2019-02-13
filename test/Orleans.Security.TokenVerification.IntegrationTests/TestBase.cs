using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Orleans.Security.TokenVerification.IntegrationTests.Configuration;

namespace Orleans.Security.TokenVerification.IntegrationTests
{
    public class TestBase
    {
        private readonly HttpClient _identityServer4Client;
        
        protected DiscoveryResponse DiscoveryResponse { get; }

        protected TestBase()
        {
            var identityServer4 = TestIdentityServer4Builder.BuildNew();
            _identityServer4Client = identityServer4.CreateClient();

            DiscoveryResponse = _identityServer4Client.GetDiscoveryDocumentAsync().Result;
        }
        
        protected async Task<string> RequestClientCredentialsTokenAsync(string clientId, string clientSecret,
            string scope)
        {
            var response = await _identityServer4Client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest()
                {
                    Address = DiscoveryResponse.TokenEndpoint,
                    Scope = scope,
                    ClientId = clientId,
                    ClientSecret = clientSecret
                });

            return response.AccessToken;
        }
    }
}
