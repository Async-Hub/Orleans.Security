using System.Threading.Tasks;
using Orleans.Security.AccessToken;
using Xunit;

namespace Orleans.Security.IntegrationTests.TokenVerification
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
            var claims = JwtSecurityTokenVerifier.Verify(accessToken, scope, DiscoveryDocument);

            // Assert
            Assert.True(claims != null);
        }
    }
}
