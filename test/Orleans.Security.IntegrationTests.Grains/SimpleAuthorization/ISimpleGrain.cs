using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains.SimpleAuthorization
{
    [Authorize]
    public interface ISimpleGrain : IGrainWithGuidKey
    {
        [AllowAnonymous]
        Task<string> GetWithAnonymousUser(string secret);
        
        Task<string> GetWithAuthenticatedUser(string secret);
        
        Task<string> GetValue();
    }
}