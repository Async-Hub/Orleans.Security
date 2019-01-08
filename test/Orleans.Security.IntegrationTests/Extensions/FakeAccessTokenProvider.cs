using Orleans.Security.ClusterClient;

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
