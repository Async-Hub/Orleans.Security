using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Orleans.Security.Authorization
{
    public interface IAuthorizeHandler
    {
        Task AuthorizeAsync(IEnumerable<Claim> claims, IEnumerable<IAuthorizeData> grainAuthorizeData, 
            IEnumerable<IAuthorizeData> methodAuthorizeData);
    }
}