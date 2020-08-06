namespace Orleans.Security.AccessToken

open System.Threading.Tasks

type internal IAccessTokenIntrospectionService =
    abstract IntrospectTokenAsync: accessToken:string
     -> allowOfflineValidation:bool -> Task<AccessTokenIntrospectionResult>

//using System.Threading.Tasks;

//namespace Orleans.Security.AccessToken
//{
//    internal interface IAccessTokenIntrospectionService
//    {
//        Task<AccessTokenIntrospectionResult> IntrospectTokenAsync(string accessToken,
//            bool allowOfflineValidation = false);
//    }
//}