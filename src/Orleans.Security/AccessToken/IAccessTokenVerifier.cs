using System.Threading.Tasks;

namespace Orleans.Security.AccessToken
{
    public interface IAccessTokenVerifier
    {
        Task<AccessTokenVerificationResult> Verify(string accessToken);
    }
}
