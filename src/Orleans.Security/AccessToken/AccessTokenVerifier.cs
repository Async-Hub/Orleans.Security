using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Orleans.Security.AccessToken.Jwt;

namespace Orleans.Security.AccessToken
{
    public class AccessTokenVerifierOptions
    {
        public bool InMemoryCacheEnabled { get; set; }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class AccessTokenVerifier : IAccessTokenVerifier
    {
        private readonly ILogger<AccessTokenVerifier> _logger;

        public AccessTokenVerifier(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AccessTokenVerifier>();
        }

        public async Task<AccessTokenVerificationResult> Verify(string accessToken, 
            OAuth2EndpointInfo oAuth2EndpointInfo)
        {
            //TODO: Implement caching functionality to improve performance.

            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentException($"The value of {nameof(accessToken)} can't be null or empty.");
            }

            var accessTokenType = AccessTokenType.Reference;
            var jwtTokenParser = new JwtTokenParser();

            if (jwtTokenParser.IsAccessTokenJwt(accessToken))
            {
                accessTokenType = AccessTokenType.Jwt;
            }

            var handler = new HttpClientHandler();

#if STAGE || DEBUG
            #region Certificate validation for a self-signed certificate

#if !STAGE && !DEBUG
#error Disable certificate validation in production deployment !
#endif
            handler.ServerCertificateCustomValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
                    {
                        _logger.LogWarning("Remote certificate validation failed, it is explicitly disabled in code.");
                    }

                    return true;
                };

            #endregion
#endif
            var httpClient = new HttpClient(handler);

            var response = await httpClient.IntrospectTokenAsync(new TokenIntrospectionRequest
            {
                Address = $"{oAuth2EndpointInfo.AuthorityUrl}/connect/introspect",
                ClientId = oAuth2EndpointInfo.ClientScopeName,
                Token = accessToken,
                ClientSecret = oAuth2EndpointInfo.ClientSecret,
            });

            // ReSharper disable once InvertIf
            if (!response.IsError)
            {
                return AccessTokenVerificationResult.CreateSuccess(accessTokenType, response.Claims);
            }

            var nameOfTokenType = accessTokenType == AccessTokenType.Jwt ? "JWT" : "Reference";

            _logger.LogTrace(LoggingEvents.AccessTokenValidationFailed,
                $"{LoggingEvents.AccessTokenValidationFailed.Name} Token type: {nameOfTokenType} " +
                $"Reason: {response.Error} " +
                $"Token value: {accessToken}");

            return AccessTokenVerificationResult.CreateFailed(response.Error);
        }
    }
}