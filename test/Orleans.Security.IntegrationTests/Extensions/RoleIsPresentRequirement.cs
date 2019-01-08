using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Extensions
{
    public class RoleIsPresentRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public RoleIsPresentRequirement(string role)
        {
            Role = role;
        }
    }
}