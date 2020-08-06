namespace Orleans.Security.AccessToken

open System.Threading.Tasks

type IAccessTokenVerifier =
    abstract Verify: accessToken : string -> Task<AccessTokenVerificationResult>

//using System.Threading.Tasks;

//namespace Orleans.Security.AccessToken
//{
//    public interface IAccessTokenVerifier
//    {
//        Task<AccessTokenVerificationResult> Verify(string accessToken);
//    }
//}