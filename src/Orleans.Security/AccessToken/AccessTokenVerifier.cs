using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Orleans.Security.Caching;

namespace Orleans.Security.AccessToken
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AccessTokenVerifier : IAccessTokenVerifier
    {
        private readonly AccessTokenVerifierOptions _options;
        private readonly IAccessTokenCache _accessTokenCache;
        private readonly ILogger<AccessTokenVerifier> _logger;

        public AccessTokenVerifier(AccessTokenVerifierOptions options,
            IAccessTokenCache accessTokenCache,
            ILogger<AccessTokenVerifier> logger)
        {
            _options = options;
            _accessTokenCache = accessTokenCache;
            _logger = logger;
        }

        public async Task<AccessTokenVerificationResult> Verify(string accessToken, 
            OAuth2EndpointInfo oAuth2EndpointInfo)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException($"The value of {nameof(accessToken)} can't be null or empty.");
            }

            if (_options.InMemoryCacheEnabled)
            {
                // TODO: Try to use more optimized solution for cache key.
                // If access token is reference token it is ok. Even so, JWT token size might be significant.

                if (_accessTokenCache.Current.TryGetValue(accessToken, out var result))
                {
                    return result as AccessTokenVerificationResult;
                }
            }

            var response = await IntrospectTokenAsync(accessToken, oAuth2EndpointInfo);

            // ReSharper disable once InvertIf
            AccessTokenVerificationResult verificationResult;
            var accessTokenType = AccessTokenAnalyzer.GetType(accessToken);

            if (!response.IsError)
            {
                verificationResult = AccessTokenVerificationResult.CreateSuccess(accessTokenType, response.Claims);
            }
            else
            {
                verificationResult = AccessTokenVerificationResult.CreateFailed(response.Error);

                var nameOfTokenType = accessTokenType == AccessTokenType.Jwt ? "JWT" : "Reference";

                _logger.LogTrace(LoggingEvents.AccessTokenValidationFailed,
                    $"{LoggingEvents.AccessTokenValidationFailed.Name} Token type: {nameOfTokenType} " +
                    $"Reason: {response.Error} " +
                    $"Token value: {accessToken}");
            }

            if (!_options.InMemoryCacheEnabled)
            {
                return verificationResult;
            }

            var cacheEntry = _accessTokenCache.Current.CreateEntry(accessToken);
            cacheEntry.Value = verificationResult;
            cacheEntry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(_options.CacheEntryExpirationTime);

            return verificationResult;
        }

        private async Task<IntrospectionResponse> IntrospectTokenAsync(string accessToken, 
            OAuth2EndpointInfo oAuth2EndpointInfo)
        {
            var httpClientHandler = new HttpClientHandler();

            if (_options.DisableCertificateValidation)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) =>
                    {
                        if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
                        {
                            _logger.LogWarning(
                                "Remote certificate validation failed. It is explicitly disabled in code.");
                        }

                        return true;
                    };
            }

            var httpClient = new HttpClient(httpClientHandler);

            var response = await httpClient.IntrospectTokenAsync(new TokenIntrospectionRequest
            {
                Address = $"{oAuth2EndpointInfo.AuthorityUrl}/connect/introspect",
                ClientId = oAuth2EndpointInfo.ClientScopeName,
                Token = accessToken,
                ClientSecret = oAuth2EndpointInfo.ClientSecret,
            });

            return response;
        }
    }
}