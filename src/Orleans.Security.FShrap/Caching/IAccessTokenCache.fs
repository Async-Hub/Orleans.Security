namespace Orleans.Security.Caching

open Microsoft.Extensions.Caching.Memory

type IAccessTokenCache =
    abstract Current : IMemoryCache with get

//using Microsoft.Extensions.Caching.Memory;

//namespace Orleans.Security.Caching
//{
//    internal interface IAccessTokenCache
//    {
//        IMemoryCache Current { get; }
//    }
//}