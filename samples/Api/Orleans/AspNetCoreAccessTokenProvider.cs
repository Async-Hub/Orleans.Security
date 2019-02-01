using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Orleans.Security.Client;

namespace Api.Orleans
{
    public class AspNetCoreAccessTokenProvider : IAccessTokenProvider
    {
        private readonly Func<IHttpContextAccessor> _httpContextAccessorResolver;

        public AspNetCoreAccessTokenProvider(Func<IHttpContextAccessor> httpContextAccessorResolver)
        {
            // ReSharper disable once JoinNullCheckWithUsage
            if (httpContextAccessorResolver == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessorResolver), 
                    "The value for IHttpContextAccessor can not be null.");
            }

            _httpContextAccessorResolver = httpContextAccessorResolver;
        }

        public string RetrieveToken()
        {
            var httpContextAccessor = _httpContextAccessorResolver.Invoke();

            // The first approach
            var token1 = httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
            
            // The second approach
            var token2 = httpContextAccessor.HttpContext.Request
                .Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            return token1 ?? token2;
        }
    }
}
