using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains.PolicyBasedAuthorization
{
    public interface IPolicyGrain : IGrainWithStringKey
    {
        [Authorize(Policy = "ManagerPolicy")]
        Task<string> GetWithMangerPolicy(string secret);
    }
}