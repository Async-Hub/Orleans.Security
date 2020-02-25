using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains.ClaimsBasedAuthorization
{
    public interface IClaimGrain : IGrainWithStringKey
    {
        [Authorize(Policy = "NewYorkCityOnly")]
        Task<string> DoSomething(string someInput);
    }
}