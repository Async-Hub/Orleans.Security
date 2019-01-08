using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Extensions
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