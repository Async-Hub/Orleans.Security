using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Orleans.Security.AccessToken;
using Orleans.Security.IntegrationTests.TokenVerification.Configuration;

namespace Orleans.Security.IntegrationTests.TokenVerification
{
    public class TestBase
    {
        private readonly HttpClient _identityServer4Client;

        protected DiscoveryDocumentShortInfo DiscoveryDocument { get; }

        protected TestBase()
        {
            var identityServer4 = TestIdentityServer4Builder.BuildNew();
            _identityServer4Client = identityServer4.CreateClient();

            var discoveryResponse = _identityServer4Client.GetDiscoveryDocumentAsync().Result;

            DiscoveryDocument = new DiscoveryDocumentShortInfo
            {
                IntrospectionEndpoint = discoveryResponse.IntrospectionEndpoint,
                Issuer = discoveryResponse.Issuer,
                Keys = discoveryResponse.KeySet.Keys,
                TokenEndpoint = discoveryResponse.TokenEndpoint
            };
        }
        
        protected async Task<string> RequestClientCredentialsTokenAsync(string clientId, string clientSecret,
            string scope)
        {
            var response = await _identityServer4Client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest()
                {
                    Address = DiscoveryDocument.TokenEndpoint,
                    Scope = scope,
                    ClientId = clientId,
                    ClientSecret = clientSecret
                });

            return response.AccessToken;
        }
    }
}
