namespace Orleans.Security.Caching

open System
open Microsoft.Extensions.Caching.Memory
open Microsoft.Extensions.Options

type internal AccessTokenCache(memoryCacheOptions: IOptions<MemoryCacheOptions>) =
    do
        if memoryCacheOptions = null then raise(new ArgumentNullException("memoryCacheOptions"))
    interface IAccessTokenCache with
        member val Current : IMemoryCache = new MemoryCache(memoryCacheOptions) :> IMemoryCache

//using System;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Options;

//namespace Orleans.Security.Caching
//{
//    internal class AccessTokenCache : IAccessTokenCache
//    {
//        internal AccessTokenCache(IOptions<MemoryCacheOptions> memoryCacheOptions)
//        {
//            if (memoryCacheOptions == null)
//            {
//                throw new ArgumentNullException(nameof(memoryCacheOptions));
//            }

//            Current = new MemoryCache(memoryCacheOptions);
//        }

//        public IMemoryCache Current { get; }
//    }
//}