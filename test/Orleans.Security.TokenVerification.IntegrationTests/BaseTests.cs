using System;
using System.Net.Http;
using System.Threading.Tasks;
using Orleans.Security.AccessToken;
using Xunit;

namespace Orleans.Security.TokenVerification.IntegrationTests
{
    public class BaseTests : TestBase
    {
        [Theory]
        [InlineData("WebClient", "Secret", "Api1")]
        public async Task RetrieveAccessToken_ShouldBeSuccessful(string clientId, string clientSecret, string scope)
        {
            // Arrange
            // Act
            var accessToken = await RequestJwtTokenAsync(clientId, clientSecret, scope);
            var accessTokenType = AccessTokenAnalyzer.GetTokenType(accessToken);
            
            // Assert
            Assert.Equal(expected: AccessTokenType.Jwt, actual: accessTokenType);
        }
    }
}
