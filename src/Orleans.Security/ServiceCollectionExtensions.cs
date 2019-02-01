using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans.Security.AccessToken;
using Orleans.Security.Authorization;

namespace Orleans.Security
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddOrleansClusterSecurityServices(this IServiceCollection services,
            Action<IServiceCollection> configureServices = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(ServiceDescriptor.Singleton<IAuthorizeHandler, AuthorizeHandler>());

            configureServices?.Invoke(services);

            services.TryAdd(ServiceDescriptor.Singleton<IAccessTokenVerifier, AccessTokenVerifier>());
        }
    }
}