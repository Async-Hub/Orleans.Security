using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Security.AccessToken;
using Orleans.Security.Authorization;

namespace Orleans.Security.CoHosting
{
    internal class OutgoingGrainCallAuthorizationFilter : GrainAuthorizationFilterBase, IOutgoingGrainCallFilter
    {
        private readonly IAccessTokenProvider _accessTokenProvider;

        private readonly IdentityServer4Info _identityServer4Info;

        public OutgoingGrainCallAuthorizationFilter(IAccessTokenProvider accessTokenProvider,
            IAccessTokenVerifier accessTokenVerifier,
            IdentityServer4Info identityServer4Info, IAuthorizationExecutor authorizeHandler,
            ILoggerFactory loggerFactory) : base(accessTokenVerifier, authorizeHandler)
        {
            _accessTokenProvider = accessTokenProvider;
            _identityServer4Info = identityServer4Info;

            Logger = loggerFactory.CreateLogger<OutgoingGrainCallAuthorizationFilter>();
        }

        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            if (AuthenticationChallenge(context))
            {
                var accessToken = await _accessTokenProvider.RetrieveTokenAsync();

                await AuthorizeAsync(context, accessToken);

                var grainType = context.Grain.GetType();

                Logger.LogTrace(LoggingEvents.OutgoingGrainCallAuthorizationPassed,
                    $"{LoggingEvents.OutgoingGrainCallAuthorizationPassed.Name} Type of Grain: {grainType.Name} " +
                    $"Method Name: {context.InterfaceMethod.Name} ");

                RequestContext.Set(ConfigurationKeys.AccessTokenKey, accessToken);
            }

            await context.Invoke();
        }
    }
}
