using System.Threading.Tasks;

namespace Orleans.Security.IntegrationTests.Grains.PolicyBasedAuthorization
{
    public class PolicyGrainGrain : Grain, IPolicyGrain
    {
        public Task<string> GetWithMangerPolicy(string secret)
        {
            return Task.FromResult(secret);
        }
    }
}