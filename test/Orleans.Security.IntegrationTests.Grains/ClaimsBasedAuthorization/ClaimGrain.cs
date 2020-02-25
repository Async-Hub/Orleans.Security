using System.Threading.Tasks;

namespace Orleans.Security.IntegrationTests.Grains.ClaimsBasedAuthorization
{
    public class ClaimGrain : Grain, IClaimGrain
    {
        public Task<string> DoSomething(string someInput)
        {
            return Task.FromResult<string>(someInput);
        }
    }
}