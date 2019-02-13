using System;
using System.Threading.Tasks;
using IdentityModel;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Configuration
{
    public class EmailVerifiedHandler : AuthorizationHandler<EmailVerifiedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            EmailVerifiedRequirement requirement)
        {
            // ReSharper disable once InvertIf
            if (context.User.HasClaim(c => c.Type == JwtClaimTypes.EmailVerified))
            {
                var claim = context.User.FindFirst(c => c.Type == JwtClaimTypes.EmailVerified);
                var isEmailVerified = Convert.ToBoolean(claim.Value);

                if (isEmailVerified == requirement.IsEmailVerified)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}