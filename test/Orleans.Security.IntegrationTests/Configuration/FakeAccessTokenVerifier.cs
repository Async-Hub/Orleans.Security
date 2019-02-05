using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using NUnit.Framework;
using Orleans.Security.AccessToken;

namespace Orleans.Security.IntegrationTests.Configuration
{
    internal class FakeAccessTokenVerifier : IAccessTokenVerifier
    {
        public static LoggedInUser LoggedInUser { private get; set; }

        public Task<AccessTokenVerificationResult> Verify(string accessToken)
        {
            var userName = TestContext.CurrentContext.Test.FullName;
            var loggedInUser = (int) LoggedInUser;

            if (!AuthorizationTestConfig.Claims.ContainsKey(loggedInUser))
            {
                throw new InvalidOperationException($"Invalid LoggedInUser value for {userName}.");
            }

            // ReSharper disable once SuggestVarOrType_Elsewhere
            IEnumerable<Claim> claims = AuthorizationTestConfig.Claims[loggedInUser];

            return Task.FromResult(AccessTokenVerificationResult.CreateSuccess(AccessTokenType.Jwt,
                claims));
        }
    }
}
