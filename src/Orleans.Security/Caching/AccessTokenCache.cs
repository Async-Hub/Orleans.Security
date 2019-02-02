using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Orleans.Security.Caching
{
    internal class AccessTokenCache : IAccessTokenCache
    {
        internal AccessTokenCache(IOptions<MemoryCacheOptions> memoryCacheOptions)
        {
            if (memoryCacheOptions == null)
            {
                throw new ArgumentNullException(nameof(memoryCacheOptions));
            }

            Current = new MemoryCache(memoryCacheOptions);
        }

        public IMemoryCache Current { get; }
    }
}