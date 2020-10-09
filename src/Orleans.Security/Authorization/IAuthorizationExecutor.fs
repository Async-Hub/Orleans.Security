namespace Orleans.Security.Authorization

open System.Collections.Generic
open System.Security.Claims
open System.Threading.Tasks

type internal IAuthorizationExecutor =
    abstract member AuthorizeAsync : 
        claims: IEnumerable<Claim> * 
        grainInterfaceAuthorizeData: IEnumerable<IAuthorizeData> * 
        grainMethodAuthorizeData: IEnumerable<IAuthorizeData> -> Task<bool>

//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Orleans.Security.Authorization
//{
//    public interface IAuthorizationExecutor
//    {
//        Task AuthorizeAsync(IEnumerable<Claim> claims, IEnumerable<IAuthorizeData> grainInterfaceAuthorizeData, 
//            IEnumerable<IAuthorizeData> grainMethodAuthorizeData);
//    }
//}
