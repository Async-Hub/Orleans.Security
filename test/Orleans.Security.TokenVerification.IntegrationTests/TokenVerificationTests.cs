using System;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Orleans.Security.AccessToken;
using Xunit;

namespace Orleans.Security.TokenVerification.IntegrationTests
{
    public class TokenVerificationTests : TestBase
    {
        [Theory]
        [InlineData("WebClient", "Secret", "Api1")]
        public async Task VerifyAccessToken_WithCorrectScope_ShouldBeSuccessful(string clientId,
            string clientSecret, string scope)
        {
            // Arrange
            var accessToken = await RequestClientCredentialsTokenAsync(clientId, clientSecret, scope);

            // Act
            var claims = JwtVerifier.Verify(accessToken, scope, DiscoveryResponse);

            // Assert
            Assert.True(claims != null);
        }
    }
}
