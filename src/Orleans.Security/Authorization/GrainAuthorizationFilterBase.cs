using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Security.AccessToken;

namespace Orleans.Security.Authorization
{
    public abstract class GrainAuthorizationFilterBase
    {
        private readonly IAuthorizeHandler _authorizeHandler;

        private readonly IAccessTokenVerifier _accessTokenVerifier;

        protected ILogger Logger;

        protected GrainAuthorizationFilterBase(IAccessTokenVerifier accessTokenVerifier, 
            IAuthorizeHandler authorizeHandler)
        {
            _authorizeHandler = authorizeHandler;
            _accessTokenVerifier = accessTokenVerifier;
        }

        public bool AuthenticationChallenge(IGrainCallContext grainCallContext)
        {
            var allowAnonymousAttribute = 
                grainCallContext.InterfaceMethod.GetCustomAttribute<AllowAnonymousAttribute>();
            
            // No authorization required.
            if (allowAnonymousAttribute != null)
            { 
                return false;
            }

            IEnumerable<IAuthorizeData> grainAuthorizeData = null;
            var grainMethodAuthorizeData = grainCallContext.InterfaceMethod.GetCustomAttributes<AuthorizeAttribute>();

            if (grainCallContext.InterfaceMethod.ReflectedType != null)
            {
                grainAuthorizeData =
                    grainCallContext.InterfaceMethod.ReflectedType.GetCustomAttributes<AuthorizeAttribute>();
            }

            // No authorization required.
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (grainAuthorizeData != null && !grainAuthorizeData.Any() && !grainMethodAuthorizeData.Any())
            {
                return false;
            }

            return true;
        }

        protected async Task AuthorizeAsync(IGrainCallContext grainCallContext, 
            string accessToken, OAuth2EndpointInfo oAuth2EndpointInfo)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException($"{nameof(accessToken)}");
            }

            if (oAuth2EndpointInfo == null)
            {
                throw new ArgumentNullException($"{nameof(oAuth2EndpointInfo)}");
            }

            var accessTokenVerificationResult = await _accessTokenVerifier.Verify(accessToken, oAuth2EndpointInfo);

            if (accessTokenVerificationResult.IsVerified)
            {
                IEnumerable<IAuthorizeData> grainAuthorizeData = null;
                var grainMethodAuthorizeData = grainCallContext.InterfaceMethod.GetCustomAttributes<AuthorizeAttribute>();

                if (grainCallContext.InterfaceMethod.ReflectedType != null)
                {
                    grainAuthorizeData =
                        grainCallContext.InterfaceMethod.ReflectedType.GetCustomAttributes<AuthorizeAttribute>();
                }

                await _authorizeHandler.AuthorizeAsync(accessTokenVerificationResult.Claims,
                    grainAuthorizeData, grainMethodAuthorizeData);
            }
            else
            {
                throw new OrleansClusterUnauthorizedAccessException("Access token verification failed.",
                    new InvalidAccessTokenException(accessTokenVerificationResult.InvalidValidationMessage));
            }
        }
    }
}
