using System.Threading.Tasks;
using NUnit.Framework;
using Orleans.Security.IntegrationTests.Configuration;

namespace Orleans.Security.IntegrationTests
{
    [SetUpFixture]
    public class SetUpTestEnvironment
    {
        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            // Start test cluster.
            await TestClusterBuilder.StartSilo();

            // Start test client.
            await TestClientBuilder.StartClient();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
