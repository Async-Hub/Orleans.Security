using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Security.AccessToken;

namespace Orleans.Security.Authorization
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class IncomingGrainCallAuthorizationFilter : GrainAuthorizationFilterBase, IIncomingGrainCallFilter
    {
        public IncomingGrainCallAuthorizationFilter(IAccessTokenVerifier accessTokenVerifier,
            // ReSharper disable once SuggestBaseTypeForParameter
            IAuthorizationExecutor authorizeHandler, ILogger<IncomingGrainCallAuthorizationFilter> logger)
            : base(accessTokenVerifier, authorizeHandler)
        {
            Logger = logger;
        }

        public async Task Invoke(IIncomingGrainCallContext context)
        {
            if (AuthorizationAdmission.IsRequired(context))
            {
                await AuthorizeAsync(context);
                
                var grainType = context.Grain.GetType();
                Log(LoggingEvents.IncomingGrainCallAuthorizationPassed,
                    grainType.Name, context.InterfaceMethod.Name);
            }

            await context.Invoke();
        }
    }
}
