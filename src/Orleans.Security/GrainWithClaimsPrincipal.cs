using System.Security.Claims;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.Security
{
    public class GrainWithClaimsPrincipal : Grain, IIncomingGrainCallFilter
    {
        protected ClaimsPrincipal User;
        
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            User = (ClaimsPrincipal)RequestContext.Get(ConfigurationKeys.ClaimsPrincipalKey);
            
            await context.Invoke();
        }
    }
}