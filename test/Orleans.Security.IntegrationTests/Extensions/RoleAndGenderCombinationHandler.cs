using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Orleans.Security.Authorization;

namespace Orleans.Security.IntegrationTests.Extensions
{
    public class RoleAndGenderCombinationHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var requirement in pendingRequirements)
            {
                switch (requirement)
                {
                    case RoleIsPresentRequirement roleIsPresentRequirement:
                    {
                        if (context.User.IsInRole(roleIsPresentRequirement.Role))
                        {
                            context.Succeed(roleIsPresentRequirement);
                        }

                        break;
                    }
                    case GenderRequirement genderRequirement:
                    {
                        if (context.User.HasClaim(c => c.Type == JwtClaimTypes.Gender))
                        {
                            var claim = context.User.FindFirst(c => c.Type == JwtClaimTypes.Gender);
                            if (claim.Value == genderRequirement.Gender)
                            {
                                context.Succeed(requirement);
                            }
                        }

                        break;
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}