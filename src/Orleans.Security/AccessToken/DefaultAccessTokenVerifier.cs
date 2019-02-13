using System;
using System.Threading.Tasks;
using Orleans.Security.Caching;

namespace Orleans.Security.AccessToken
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class DefaultAccessTokenVerifier : IAccessTokenVerifier
    {
        private readonly AccessTokenVerifierOptions _options;

        private readonly IAccessTokenCache _accessTokenCache;

        private readonly IAccessTokenIntrospectionService _introspectionService;

        public DefaultAccessTokenVerifier(AccessTokenVerifierOptions options,
            IAccessTokenCache accessTokenCache,
            IAccessTokenIntrospectionService introspectionService)
        {
            _options = options;
            _accessTokenCache = accessTokenCache;
            _introspectionService = introspectionService;
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

            var introspectionResult = await _introspectionService.IntrospectTokenAsync(accessToken);

            // ReSharper disable once InvertIf
            AccessTokenVerificationResult verificationResult;

            if (introspectionResult.IsValid)
            {
                verificationResult = AccessTokenVerificationResult.CreateSuccess(introspectionResult.AccessTokenType, 
                    introspectionResult.Claims);
            }
            else
            {
                verificationResult = AccessTokenVerificationResult.CreateFailed(introspectionResult.Message);
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