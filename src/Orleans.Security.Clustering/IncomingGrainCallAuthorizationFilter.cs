using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Security.AccessToken;
using Orleans.Security.Authorization;

namespace Orleans.Security.Clustering
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class IncomingGrainCallAuthorizationFilter : GrainAuthorizationFilterBase, IIncomingGrainCallFilter
    {
        public IncomingGrainCallAuthorizationFilter(IAccessTokenVerifier accessTokenVerifier,
            IAuthorizeHandler authorizeHandler, ILoggerFactory loggerFactory)
            : base(accessTokenVerifier, authorizeHandler)
        {
            Logger = loggerFactory.CreateLogger<IncomingGrainCallAuthorizationFilter>();
        }

        public async Task Invoke(IIncomingGrainCallContext context)
        {
            if (AuthenticationChallenge(context))
            {
                var accessToken = RequestContext.Get(ConfigConstants.AccessTokenKey).ToString();
                var oidcEndpointInfo = (OAuth2EndpointInfo) RequestContext.Get(ConfigConstants.OAuth2EndpointInfoKey);

                await AuthorizeAsync(context, accessToken, oidcEndpointInfo);
            }

            await context.Invoke();
        }
    }
}
