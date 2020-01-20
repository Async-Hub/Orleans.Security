using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains
{
    public interface IUserGrain : IGrainWithStringKey
    {
        [Authorize(Policy = "ManagerPolicy")]
        Task<string> TakeSecret();
    }
}
