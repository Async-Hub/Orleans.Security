using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;

namespace Orleans.Security.AccessToken
{
    internal class TokenIntrospectionClient
    {
        private readonly HttpClient _httpClient;
        private readonly IdentityServer4Info _identityServer4Info;
        private readonly ILogger<TokenIntrospectionClient> _logger;

        public TokenIntrospectionClient(HttpClient httpClient, 
            IdentityServer4Info identityServer4Info, ILogger<TokenIntrospectionClient> logger)
        {
            _httpClient = httpClient;
            _identityServer4Info = identityServer4Info;
            _logger = logger;
        }

        public async Task<IntrospectionResponse> IntrospectTokenAsync(string accessToken,
            bool disableCertificateValidation = false)
        {
            var response = await _httpClient.IntrospectTokenAsync(new TokenIntrospectionRequest
            {
                Address = _identityServer4Info.TokenIntrospectionEndpointUrl,
                ClientId = _identityServer4Info.ClientScopeName,
                Token = accessToken,
                ClientSecret = _identityServer4Info.ClientSecret,
            });

            return response;
        }
    }
}