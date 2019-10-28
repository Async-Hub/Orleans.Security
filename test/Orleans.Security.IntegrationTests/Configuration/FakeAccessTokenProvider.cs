using System.Threading.Tasks;
using Orleans.Security.Client;

namespace Orleans.Security.IntegrationTests.Configuration
{
    internal class FakeAccessTokenProvider : IAccessTokenProvider
    {
        public Task<string> RetrieveTokenAsync()
        {
            return Task.FromResult("Fake Token");
        }
    }
}
