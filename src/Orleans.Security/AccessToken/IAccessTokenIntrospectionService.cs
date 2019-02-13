using System.Threading.Tasks;

namespace Orleans.Security.AccessToken
{
    internal interface IAccessTokenIntrospectionService
    {
        Task<AccessTokenIntrospectionResult> IntrospectTokenAsync(string accessToken,
            bool allowOfflineValidation = false);
    }
}