using System.Threading.Tasks;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.TestGrains
{
    [Authorize(Policy = "ManagerPolicy")]
    public interface IAuthorizationTestGrain : IGrainWithStringKey
    {
        [Authorize(Policy = "EmailVerifiedPolicy")]
        Task<string> TakeForEmailVerifiedPolicy(string someString);

        [Authorize(Roles = "Developer")]
        [Authorize(Roles = "Manager")]
        Task<string> TakeForCombinedRoles(string someString);

        [Authorize(Roles = "FemaleAdminPolicy")]
        Task<string> TakeForFemaleAdminPolicy(string someString);

        [Authorize(Policy = "FemaleManagerPolicy")]
        Task<string> TakeForFemaleManagerPolicy(string someString);

        [Authorize(Policy = "FemaleManagerPolicy")]
        [Authorize(Roles = "Admin")]
        Task<string> TakeForFemaleManagerPolicyAndAdminRole(string someString);

        [Authorize(Policy = "AdminPolicy")]
        [Authorize(Roles = "Admin")]
        Task<string> TakePrivateData(string someString);

        [AllowAnonymous]
        Task<string> TakePublicData(string someString);
    }
}