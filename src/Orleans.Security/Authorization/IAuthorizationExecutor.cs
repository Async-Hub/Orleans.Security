using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Orleans.Security.Authorization
{
    public interface IAuthorizationExecutor
    {
        Task AuthorizeAsync(IEnumerable<Claim> claims, IEnumerable<IAuthorizeData> grainInterfaceAuthorizeData, 
            IEnumerable<IAuthorizeData> grainMethodAuthorizeData);
    }
}