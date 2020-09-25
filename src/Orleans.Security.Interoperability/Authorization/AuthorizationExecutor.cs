using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;

namespace Orleans.Security.Authorization
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AuthorizationExecutor : IAuthorizationExecutor
    {
        private readonly IAuthorizationPolicyProvider _policyProvider;

        private readonly IAuthorizationService _authorizationService;

        public AuthorizationExecutor(IAuthorizationPolicyProvider policyProvider,
            IAuthorizationService authorizationService)
        {
            _policyProvider = policyProvider;
            _authorizationService = authorizationService;
        }

        public async Task AuthorizeAsync(IEnumerable<Claim> claims,
            IEnumerable<IAuthorizeData> grainInterfaceAuthorizeData,
            IEnumerable<IAuthorizeData> grainMethodAuthorizeData)
        {
            var claimsIdentity = new ClaimsIdentity(claims, "AccessToken",
                JwtClaimTypes.Subject, JwtClaimTypes.Role);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authorizationSucceeded = true;

            if (grainMethodAuthorizeData != null)
            {
                var authorizeData1 = grainMethodAuthorizeData as IAuthorizeData[] ??
                                     grainMethodAuthorizeData.ToArray();

                if (authorizeData1.Any())
                {
                    var policy = await AuthorizationPolicy.CombineAsync(_policyProvider, authorizeData1);
                    var authorizationResult = await _authorizationService
                        .AuthorizeAsync(claimsPrincipal, policy);

                    authorizationSucceeded = authorizationResult.Succeeded;
                }
            }

            if (grainInterfaceAuthorizeData != null)
            {
                var authorizeData2 = grainInterfaceAuthorizeData as IAuthorizeData[] ??
                                     grainInterfaceAuthorizeData.ToArray();

                if (authorizationSucceeded && authorizeData2.Any())
                {
                    var policy = await AuthorizationPolicy.CombineAsync(_policyProvider, authorizeData2);
                    var authorizationResult = await _authorizationService
                        .AuthorizeAsync(claimsPrincipal, policy);

                    authorizationSucceeded = authorizationResult.Succeeded;
                }
            }

            if (!authorizationSucceeded)
            {
                throw new OrleansClusterUnauthorizedAccessException("Access to the requested grain denied.");
            }
        }
    }
}
