using System.Threading.Tasks;

namespace Orleans.Security.IntegrationTests.GrainsForTests
{
    // ReSharper disable once UnusedMember.Global
    public class AuthorizationTestGrain : Grain, IAuthorizationTestGrain
    {
        public Task<string> TakeForEmailVerifiedPolicy(string someString)
        {
            return Task.FromResult(someString);
        }

        public Task<string> TakeForCombinedRoles(string someString)
        {
            return Task.FromResult(someString);
        }

        public Task<string> TakeForFemaleAdminPolicy(string someString)
        {
            return Task.FromResult(someString);
        }

        public Task<string> TakeForFemaleManagerPolicy(string someString)
        {
            return Task.FromResult(someString);
        }

        public Task<string> TakeForFemaleManagerPolicyAndAdminRole(string someString)
        {
            return Task.FromResult(someString);
        }

        public Task<string> TakePrivateData(string someString)
        {
            return Task.FromResult(someString);
        }

        public Task<string> TakePublicData(string someString)
        {
            return Task.FromResult(someString);
        }
    }
}