using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;

namespace Orleans.Security.Authorization
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AuthorizeHandler : IAuthorizeHandler
    {
        private readonly IAuthorizationPolicyProvider _policyProvider;

        private readonly IAuthorizationService _authorizationService;

        public AuthorizeHandler(IAuthorizationPolicyProvider policyProvider, 
            IAuthorizationService authorizationService)
        {
            _policyProvider = policyProvider;
            _authorizationService = authorizationService;
        }

        public async Task AuthorizeAsync(IEnumerable<Claim> claims, IEnumerable<IAuthorizeData> grainAuthorizeData,
            IEnumerable<IAuthorizeData> grainMethodAuthorizeData)
        {
            var claimsIdentity = new ClaimsIdentity(claims, "AccessToken",
                JwtClaimTypes.Subject, JwtClaimTypes.Role);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authorizationPassed = true;

            if (grainMethodAuthorizeData != null)
            {
                var authorizeData1 = grainMethodAuthorizeData as IAuthorizeData[] ?? grainMethodAuthorizeData.ToArray();

                if (authorizeData1.Any())
                {
                    var policy = await AuthorizationPolicy.CombineAsync(_policyProvider, authorizeData1);
                    var authorizationResult = await _authorizationService
                        .AuthorizeAsync(claimsPrincipal, policy);

                    authorizationPassed = authorizationResult.Succeeded;
                }
            }

            if (grainAuthorizeData != null)
            {
                var authorizeData2 = grainAuthorizeData as IAuthorizeData[] ?? grainAuthorizeData.ToArray();

                if (authorizationPassed && authorizeData2.Any())
                {
                    var policy = await AuthorizationPolicy.CombineAsync(_policyProvider, authorizeData2);
                    var authorizationResult = await _authorizationService
                        .AuthorizeAsync(claimsPrincipal, policy);

                    authorizationPassed = authorizationResult.Succeeded;
                }
            }

            if (!authorizationPassed)
            {
                throw new OrleansClusterUnauthorizedAccessException("Access denied.");
            }
        }
    }
}
