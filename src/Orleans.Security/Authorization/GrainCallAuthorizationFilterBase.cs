using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Security.AccessToken;

namespace Orleans.Security.Authorization
{
    public abstract class GrainCallAuthorizationFilterBase
    {
        private readonly IAuthorizeHandler _authorizeHandler;

        private readonly IAccessTokenValidator _accessTokenValidator;

        protected ILogger Logger;

        protected GrainCallAuthorizationFilterBase(IAccessTokenValidator accessTokenValidator, 
            IAuthorizeHandler authorizeHandler)
        {
            _authorizeHandler = authorizeHandler;
            _accessTokenValidator = accessTokenValidator;
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

            var accessTokenValidationResult = await _accessTokenValidator.Validate(accessToken, oAuth2EndpointInfo);

            if (accessTokenValidationResult.IsValid)
            {
                IEnumerable<IAuthorizeData> grainAuthorizeData = null;
                var grainMethodAuthorizeData = grainCallContext.InterfaceMethod.GetCustomAttributes<AuthorizeAttribute>();

                if (grainCallContext.InterfaceMethod.ReflectedType != null)
                {
                    grainAuthorizeData =
                        grainCallContext.InterfaceMethod.ReflectedType.GetCustomAttributes<AuthorizeAttribute>();
                }

                await _authorizeHandler.AuthorizeAsync(accessTokenValidationResult.Claims,
                    grainAuthorizeData, grainMethodAuthorizeData);
            }
            else
            {
                throw new OrleansClusterUnauthorizedAccessException("Invalid Access Token.",
                    new InvalidAccessTokenException(accessTokenValidationResult.InvalidValidationMessage));
            }
        }
    }
}
