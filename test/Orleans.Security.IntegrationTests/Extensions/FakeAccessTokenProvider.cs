using Orleans.Security.Client;

namespace Orleans.Security.IntegrationTests.Extensions
{
    internal class FakeAccessTokenProvider : IAccessTokenProvider
    {
        public string RetrieveToken()
        {
            return "Fake Token";
        }
    }
}
