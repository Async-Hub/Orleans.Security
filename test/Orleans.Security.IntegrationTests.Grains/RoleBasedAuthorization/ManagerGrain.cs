using System.Threading.Tasks;

namespace Orleans.Security.IntegrationTests.Grains.RoleBasedAuthorization
{
    public class ManagerGrain : Grain, IManagerGrain
    {
        public Task<string> GetWithCommaSeparatedRoles(string secret)
        {
            return Task.FromResult(secret);
        }

        public Task<string> GetWithMultipleRoleAttributes(string secret)
        {
            return Task.FromResult(secret);
        }
    }
}