using System.Collections.Generic;
using System.Security.Claims;

namespace Orleans.Security.AccessToken
{
    public class AccessTokenVerificationResult
    {
        private AccessTokenVerificationResult(bool isVerified)
        {
            IsVerified = isVerified;
        }

        public IEnumerable<Claim> Claims { get; private set; }

        public string InvalidValidationMessage { get; private set; }

        public bool IsVerified { get; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private AccessTokenType? TokenType { get; set; }

        public static AccessTokenVerificationResult CreateFailed(string message)
        {
            return new AccessTokenVerificationResult(false)
            {
                InvalidValidationMessage = message
            };
        }

        public static AccessTokenVerificationResult CreateSuccess(AccessTokenType accessTokenType,
            IEnumerable<Claim> claims)
        {
            return new AccessTokenVerificationResult(true)
            {
                Claims = claims,
                TokenType = accessTokenType
            };
        }
    }
}