using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Security.Caching;

namespace Orleans.Security.AccessToken
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class DefaultAccessTokenVerifier : IAccessTokenVerifier
    {
        private readonly AccessTokenVerifierOptions _options;

        private readonly IAccessTokenCache _accessTokenCache;

        private readonly TokenIntrospectionClient _introspectionClient;

        private readonly ILogger<DefaultAccessTokenVerifier> _logger;

        public DefaultAccessTokenVerifier(AccessTokenVerifierOptions options,
            IAccessTokenCache accessTokenCache,
            TokenIntrospectionClient introspectionClient,
            ILogger<DefaultAccessTokenVerifier> logger)
        {
            _options = options;
            _accessTokenCache = accessTokenCache;
            _introspectionClient = introspectionClient;
            _logger = logger;
        }

        public async Task<AccessTokenVerificationResult> Verify(string accessToken)
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

            var response = await _introspectionClient.IntrospectTokenAsync(accessToken);

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
    }
}