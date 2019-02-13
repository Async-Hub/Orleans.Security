using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Configuration
{
    public class EmailVerifiedRequirement : IAuthorizationRequirement
    {
        public bool IsEmailVerified { get; }

        public EmailVerifiedRequirement(bool isEmailVerified)
        {
            IsEmailVerified = isEmailVerified;
        }
    }
}