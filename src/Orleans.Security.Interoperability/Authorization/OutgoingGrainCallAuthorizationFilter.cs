using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Security.AccessToken;

namespace Orleans.Security.Authorization
{
    internal class OutgoingGrainCallAuthorizationFilter : GrainAuthorizationFilterBase, IOutgoingGrainCallFilter
    {
        public OutgoingGrainCallAuthorizationFilter(IAccessTokenVerifier accessTokenVerifier, 
            IAuthorizationExecutor authorizeHandler,
            // ReSharper disable once SuggestBaseTypeForParameter
            ILogger<OutgoingGrainCallAuthorizationFilter> logger) : base(accessTokenVerifier, authorizeHandler)
        {
            Logger = logger;
        }


        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            if (AuthorizationAdmission.IsRequired(context))
            {
                await AuthorizeAsync(context);

                var grainType = context.Grain.GetType();
                Log(LoggingEvents.OutgoingGrainCallAuthorizationPassed,
                    grainType.Name, context.InterfaceMethod.Name);
            }

            await context.Invoke();
        }
    }
}
