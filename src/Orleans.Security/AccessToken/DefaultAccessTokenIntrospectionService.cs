using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;

namespace Orleans.Security.AccessToken
{
    internal class DefaultAccessTokenIntrospectionService : IAccessTokenIntrospectionService
    {
        private readonly HttpClient _httpClient;
        private readonly IdentityServer4Info _identityServer4Info;
        private readonly IdS4DiscoveryDocumentProvider _discoveryDocumentProvider;
        private readonly ILogger<DefaultAccessTokenIntrospectionService> _logger;

        public DefaultAccessTokenIntrospectionService(IHttpClientFactory clientFactory,
            IdentityServer4Info identityServer4Info,
            IdS4DiscoveryDocumentProvider discoveryDocumentProvider,
            ILogger<DefaultAccessTokenIntrospectionService> logger)
        {
            _httpClient = clientFactory.CreateClient("IdS4");
            _identityServer4Info = identityServer4Info;
            _discoveryDocumentProvider = discoveryDocumentProvider;
            _logger = logger;
        }

        public async Task<AccessTokenIntrospectionResult> IntrospectTokenAsync(string accessToken, 
            bool allowOfflineValidation = false)
        {
            var accessTokenType = AccessTokenAnalyzer.GetTokenType(accessToken);
            var discoveryResponse = await _discoveryDocumentProvider.GetDiscoveryDocumentAsync();

            if (accessTokenType == AccessTokenType.Jwt && allowOfflineValidation)
            {
                var claims = JwtSecurityTokenVerifier.Verify(accessToken,
                    _identityServer4Info.AllowedScope, discoveryResponse);

                return new AccessTokenIntrospectionResult(accessTokenType, claims, true);
            }

            var introspectionResult = await
                IntrospectTokenOnlineAsync(accessToken, accessTokenType, discoveryResponse);

            return introspectionResult;
        }
        
        private async Task<AccessTokenIntrospectionResult> IntrospectTokenOnlineAsync(string accessToken, 
            AccessTokenType accessTokenType, DiscoveryResponse discoveryDocument)
        {
            var introspectionResponse = await _httpClient.IntrospectTokenAsync(new TokenIntrospectionRequest
            {
                Address = discoveryDocument.IntrospectionEndpoint,
                ClientId = _identityServer4Info.ClientId,
                Token = accessToken,
                ClientSecret = _identityServer4Info.ClientSecret,
            });

            var nameOfTokenType = accessTokenType == AccessTokenType.Jwt ? "JWT" : "Reference";
            
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (!introspectionResponse.IsError)
            {
                return new AccessTokenIntrospectionResult(accessTokenType, introspectionResponse.Claims,
                    introspectionResponse.IsActive);
            }

            _logger.LogTrace(LoggingEvents.AccessTokenValidationFailed,
                $"{LoggingEvents.AccessTokenValidationFailed.Name} Token type: {nameOfTokenType} " +
                $"Reason: {introspectionResponse.Error} " +
                $"Token value: {accessToken}");
                
            return new AccessTokenIntrospectionResult(accessTokenType, introspectionResponse.Claims, false);
        }
    }
}