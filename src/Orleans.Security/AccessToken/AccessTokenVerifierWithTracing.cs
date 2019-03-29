using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Orleans.Security.AccessToken
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AccessTokenVerifierWithTracing : IAccessTokenVerifier
    {
        private readonly IAccessTokenVerifier _accessTokenVerifier;

        private readonly ILogger<IAccessTokenVerifier> _logger;

        private readonly bool _isCachingEnabled;

        public AccessTokenVerifierWithTracing(bool isCachingEnabled,
            IAccessTokenVerifier accessTokenVerifier,
            ILogger<IAccessTokenVerifier> logger)
        {
            _logger = logger;
            _isCachingEnabled = isCachingEnabled;
            _accessTokenVerifier = accessTokenVerifier;
        }

        public async Task<AccessTokenVerificationResult> Verify(string accessToken)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = await _accessTokenVerifier.Verify(accessToken);

            stopwatch.Stop();
            _logger.LogInformation(LoggingEvents.AccessTokenVerified,$"Time: " +
                                   $"{stopwatch.ElapsedMilliseconds} ms. CachingEnabled: {_isCachingEnabled}");

            return result;
        }
    }
}
