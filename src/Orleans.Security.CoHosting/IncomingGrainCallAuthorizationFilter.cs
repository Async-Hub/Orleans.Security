using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Security.AccessToken;
using Orleans.Security.Authorization;

namespace Orleans.Security.CoHosting
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class IncomingGrainCallAuthorizationFilter : GrainAuthorizationFilterBase, IIncomingGrainCallFilter
    {
        public IncomingGrainCallAuthorizationFilter(IAccessTokenVerifier accessTokenVerifier,
            IAuthorizationExecutor authorizeHandler, ILoggerFactory loggerFactory)
            : base(accessTokenVerifier, authorizeHandler)
        {
            Logger = loggerFactory.CreateLogger<IncomingGrainCallAuthorizationFilter>();
        }

        public async Task Invoke(IIncomingGrainCallContext context)
        {
            if (AuthenticationChallenge(context))
            {
                var accessToken = RequestContext.Get(ConfigurationKeys.AccessTokenKey).ToString();

                await AuthorizeAsync(context, accessToken);
            }

            await context.Invoke();
        }
    }
}
