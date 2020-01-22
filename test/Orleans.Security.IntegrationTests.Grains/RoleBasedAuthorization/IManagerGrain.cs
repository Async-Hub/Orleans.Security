using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Grains.RoleBasedAuthorization
{
    public interface IManagerGrain : IGrainWithStringKey
    {
        [Authorize(Roles = "Developer, Manager")]
        Task<string> GetWithCommaSeparatedRoles(string secret);
        
        [Authorize(Roles = "Developer")]
        [Authorize(Roles = "Manager")]
        Task<string> GetWithMultipleRoleAttributes(string secret);
    }
}