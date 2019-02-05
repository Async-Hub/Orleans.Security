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
        private readonly DefaultAccessTokenVerifier _defaultAccessTokenVerifier;

        private readonly ILogger<DefaultAccessTokenVerifier> _logger;

        private readonly bool _isCachingEnabled;

        public AccessTokenVerifierWithTracing(AccessTokenVerifierOptions options,
            IAccessTokenCache accessTokenCache,
            TokenIntrospectionClient introspectionClient,
            ILogger<DefaultAccessTokenVerifier> logger)
        {
            _logger = logger;
            _isCachingEnabled = options.InMemoryCacheEnabled;
            _defaultAccessTokenVerifier = new DefaultAccessTokenVerifier(options, 
                accessTokenCache, introspectionClient, logger);
        }

        public async Task<AccessTokenVerificationResult> Verify(string accessToken)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = await _defaultAccessTokenVerifier.Verify(accessToken);

            stopwatch.Stop();
            _logger.LogInformation(LoggingEvents.AccessTokenVerified,$"Time: " +
                                   $"{stopwatch.ElapsedMilliseconds} ms. CachingEnabled: {_isCachingEnabled}");

            return result;
        }
    }
}
