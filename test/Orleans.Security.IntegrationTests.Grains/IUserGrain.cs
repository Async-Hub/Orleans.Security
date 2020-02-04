using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains
{
    [Authorize]
    public interface IUserGrain : IGrainWithStringKey
    {
        Task<string> GetWithAuthenticatedUser(string secret);
        
        [AllowAnonymous]
        Task<string> GetWithAnonymousUser(string secret);
    }
}
