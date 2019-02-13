using System.Collections.Generic;
using System.Security.Claims;

namespace Orleans.Security.AccessToken
{
    public class AccessTokenIntrospectionResult
    {
        public AccessTokenType AccessTokenType { get; }

        public IEnumerable<Claim> Claims { get; }

        public bool IsValid { get; }
        
        public string Message { get; }

        public AccessTokenIntrospectionResult(AccessTokenType accessTokenType,
            IEnumerable<Claim> claims, bool isValid, string message = null
        )
        {
            AccessTokenType = accessTokenType;
            Claims = claims;
            IsValid = isValid;
            Message = message;
        }
    }
}