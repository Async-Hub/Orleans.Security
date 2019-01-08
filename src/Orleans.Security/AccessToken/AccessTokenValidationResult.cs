using System.Collections.Generic;
using System.Security.Claims;

namespace Orleans.Security.AccessToken
{
    public class AccessTokenValidationResult
    {
        private AccessTokenValidationResult(bool isValid)
        {
            IsValid = isValid;
        }

        public IEnumerable<Claim> Claims { get; private set; }

        public string InvalidValidationMessage { get; private set; }

        public bool IsValid { get; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private AccessTokenType? TokenType { get; set; }

        public static AccessTokenValidationResult CreateFailed(string message)
        {
            return new AccessTokenValidationResult(false)
            {
                InvalidValidationMessage = message
            };
        }

        public static AccessTokenValidationResult CreateSuccess(AccessTokenType accessTokenType,
            IEnumerable<Claim> claims)
        {
            return new AccessTokenValidationResult(true)
            {
                Claims = claims,
                TokenType = accessTokenType
            };
        }
    }
}