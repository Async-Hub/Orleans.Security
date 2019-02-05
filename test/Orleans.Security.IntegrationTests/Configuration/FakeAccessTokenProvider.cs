using Orleans.Security.Client;

namespace Orleans.Security.IntegrationTests.Configuration
{
    internal class FakeAccessTokenProvider : IAccessTokenProvider
    {
        public string RetrieveToken()
        {
            return "Fake Token";
        }
    }
}
