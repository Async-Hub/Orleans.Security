using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Security.AccessToken;

namespace Orleans.Security.Authorization
{
    internal abstract class GrainAuthorizationFilterBase
    {
        private readonly IAuthorizationExecutor _authorizeHandler;

        private readonly IAccessTokenVerifier _accessTokenVerifier;

        protected ILogger Logger;

        protected GrainAuthorizationFilterBase(IAccessTokenVerifier accessTokenVerifier, 
            IAuthorizationExecutor authorizeHandler)
        {
            _authorizeHandler = authorizeHandler;
            _accessTokenVerifier = accessTokenVerifier;
        }

        protected async Task AuthorizeAsync(IGrainCallContext grainCallContext)
        {
            var accessToken = RequestContext.Get(ConfigurationKeys.AccessTokenKey).ToString();
            
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new InvalidOperationException("AccessToken can not be null or empty.");
            }

            var accessTokenVerificationResult = await _accessTokenVerifier.Verify(accessToken);

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

        protected void Log(EventId eventId, string grainTypeName, string interfaceMethodName)
        {
            Logger.LogTrace(eventId, $"{eventId.Name} Type of Grain: {grainTypeName} " +
                                     $"Method Name: {interfaceMethodName} ");
        }
    }
}
