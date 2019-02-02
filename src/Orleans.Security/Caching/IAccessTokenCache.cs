using Microsoft.Extensions.Caching.Memory;

namespace Orleans.Security.Caching
{
    internal interface IAccessTokenCache
    {
        IMemoryCache Current { get; }
    }
}
