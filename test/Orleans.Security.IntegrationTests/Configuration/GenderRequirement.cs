using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Configuration
{
    public class GenderRequirement : IAuthorizationRequirement
    {
        public string Gender { get; }

        public GenderRequirement(string gender)
        {
            Gender = gender;
        }
    }
}