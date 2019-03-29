using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Orleans.Security.Caching;

namespace Orleans.Security.AccessToken
{
    internal class AccessTokenVerifierWithCaching : IAccessTokenVerifier
    {
        private readonly IAccessTokenCache _accessTokenCache;
        private readonly int _cacheEntryExpirationTime;

        private readonly IAccessTokenVerifier _accessTokenVerifier;

        public AccessTokenVerifierWithCaching(IAccessTokenVerifier accessTokenVerifier, 
            IAccessTokenCache accessTokenCache, int cacheEntryExpirationTime)
        {
            _accessTokenVerifier = accessTokenVerifier;
            _accessTokenCache = accessTokenCache;
            _cacheEntryExpirationTime = cacheEntryExpirationTime;
        }

        public async Task<AccessTokenVerificationResult> Verify(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException($"The value of {nameof(accessToken)} can't be null or empty.");
            }

            // TODO: Need more deeper investigations, maybe this optimization is unnesesarry.
            var key = TryToOptimize(accessToken);

            if (_accessTokenCache.Current.TryGetValue(key, out var result))
            {
                return result as AccessTokenVerificationResult;
            }

            var verificationResult = await _accessTokenVerifier.Verify(accessToken);

            var cacheEntry = _accessTokenCache.Current.CreateEntry(key);
            cacheEntry.Value = verificationResult;
            cacheEntry.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(_cacheEntryExpirationTime);

            return verificationResult;
        }

        private string TryToOptimize(string accessToken)
        {
            var accessTokenType = AccessTokenAnalyzer.GetTokenType(accessToken);

            if (accessTokenType == AccessTokenType.Reference) return accessToken;

            using (var sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(accessToken));

                var sb = new StringBuilder();

                // ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 0; i < hashValue.Length; i++)
                {
                    sb.Append(hashValue[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}