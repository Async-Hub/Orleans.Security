using System.Threading.Tasks;

namespace Orleans.Security.AccessToken
{
    public interface IAccessTokenValidator
    {
        Task<AccessTokenValidationResult> Validate(string accessToken, OAuth2EndpointInfo oAuth2EndpointInfo);
    }
}
