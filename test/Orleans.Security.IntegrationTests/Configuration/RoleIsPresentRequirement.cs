using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Configuration
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