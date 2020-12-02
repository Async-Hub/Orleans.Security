namespace Orleans.Security.Authorization

open System.Collections.Generic
open System.Linq
open System.Security.Claims
open IdentityModel

type internal AuthorizationExecutor(policyProvider : IAuthorizationPolicyProvider, 
        authorizationService : IAuthorizationService) =
        interface IAuthorizationExecutor with
            member _.AuthorizeAsync(claims : IEnumerable<Claim>, 
                grainInterfaceAuthorizeData : IEnumerable<IAuthorizeData>,
                grainMethodAuthorizeData : IEnumerable<IAuthorizeData>) = 
                async {
                    let claimsIdentity = ClaimsIdentity(claims, "AccessToken",
                                            JwtClaimTypes.Subject, JwtClaimTypes.Role)

                    let claimsPrincipal = new ClaimsPrincipal(claimsIdentity)
                    let mutable authorizationSucceeded = true

                    if not (isNull grainMethodAuthorizeData) && 
                        grainMethodAuthorizeData.Any() then
                        let! policy = AuthorizationPolicy.CombineAsync(policyProvider, grainMethodAuthorizeData) |> Async.AwaitTask
                        let! authorizationResult = authorizationService
                                                    .AuthorizeAsync(claimsPrincipal, policy) |> Async.AwaitTask
                        
                        authorizationSucceeded <- authorizationResult.Succeeded

                    if not (isNull grainInterfaceAuthorizeData) && 
                        grainInterfaceAuthorizeData.Any() && authorizationSucceeded then
                        let! policy = AuthorizationPolicy.CombineAsync(policyProvider, grainInterfaceAuthorizeData) |> Async.AwaitTask
                        let! authorizationResult = authorizationService
                                                    .AuthorizeAsync(claimsPrincipal, policy) |> Async.AwaitTask
                    
                        authorizationSucceeded <- authorizationResult.Succeeded

                    return authorizationSucceeded

                } |> Async.StartAsTask

//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using IdentityModel;

//namespace Orleans.Security.Authorization
//{
//    // ReSharper disable once ClassNeverInstantiated.Global
//    public class AuthorizationExecutor : IAuthorizationExecutor
//    {
//        private readonly IAuthorizationPolicyProvider _policyProvider;

//        private readonly IAuthorizationService _authorizationService;

//        public AuthorizationExecutor(IAuthorizationPolicyProvider policyProvider,
//            IAuthorizationService authorizationService)
//        {
//            _policyProvider = policyProvider;
//            _authorizationService = authorizationService;
//        }

//        public async Task AuthorizeAsync(IEnumerable<Claim> claims,
//            IEnumerable<IAuthorizeData> grainInterfaceAuthorizeData,
//            IEnumerable<IAuthorizeData> grainMethodAuthorizeData)
//        {
//            var claimsIdentity = new ClaimsIdentity(claims, "AccessToken",
//                JwtClaimTypes.Subject, JwtClaimTypes.Role);

//            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

//            var authorizationSucceeded = true;

//            if (grainMethodAuthorizeData != null)
//            {
//                var authorizeData1 = grainMethodAuthorizeData as IAuthorizeData[] ??
//                                     grainMethodAuthorizeData.ToArray();

//                if (authorizeData1.Any())
//                {
//                    var policy = await AuthorizationPolicy.CombineAsync(_policyProvider, authorizeData1);
//                    var authorizationResult = await _authorizationService
//                        .AuthorizeAsync(claimsPrincipal, policy);

//                    authorizationSucceeded = authorizationResult.Succeeded;
//                }
//            }

//            if (grainInterfaceAuthorizeData != null)
//            {
//                var authorizeData2 = grainInterfaceAuthorizeData as IAuthorizeData[] ??
//                                     grainInterfaceAuthorizeData.ToArray();

//                if (authorizationSucceeded && authorizeData2.Any())
//                {
//                    var policy = await AuthorizationPolicy.CombineAsync(_policyProvider, authorizeData2);
//                    var authorizationResult = await _authorizationService
//                        .AuthorizeAsync(claimsPrincipal, policy);

//                    authorizationSucceeded = authorizationResult.Succeeded;
//                }
//            }

//            if (!authorizationSucceeded)
//            {
//                throw new NotAuthorizedException("Access to the requested grain denied.");
//            }
//        }
//    }
//}