using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Security.Caching;

namespace Orleans.Security.AccessToken
{
    internal class AccessTokenVerifierWithTracing : IAccessTokenVerifier
    {
        private readonly AccessTokenVerifier _accessTokenVerifier;

        private readonly ILogger<AccessTokenVerifier> _logger;

        private readonly bool _isCachingEnabled;

        public AccessTokenVerifierWithTracing(AccessTokenVerifierOptions options,
            IAccessTokenCache accessTokenCache,
            ILogger<AccessTokenVerifier> logger)
        {
            _logger = logger;
            _isCachingEnabled = options.InMemoryCacheEnabled;
            _accessTokenVerifier = new AccessTokenVerifier(options, accessTokenCache, logger);
        }

        public async Task<AccessTokenVerificationResult> Verify(string accessToken, OAuth2EndpointInfo oAuth2EndpointInfo)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = await _accessTokenVerifier.Verify(accessToken, oAuth2EndpointInfo);

            stopwatch.Stop();
            _logger.LogInformation(LoggingEvents.AccessTokenVerified,$"Time: " +
                                   $"{stopwatch.ElapsedMilliseconds} ms. CachingEnabled: {_isCachingEnabled}");

            return result;
        }
    }
}
